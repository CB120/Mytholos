using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Myths;
using StateMachines.Commands;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Participants
{
    public class PlayerParticipant : Participant
    {
        // Technically a cyclic dependency, but more effort than it's worth to solve
        [SerializeField] private PlayerParticipantRuntimeSet playerParticipantRuntimeSet;
        [SerializeField] private float deathSwapInvulnerabilityTime;
    
        //Events
        [NonSerialized] public UnityEvent<int> SelectMyth = new();
        [NonSerialized] public UnityEvent<SO_Ability> SelectAbility = new();
        [NonSerialized] public UnityEvent<PlayerParticipant> mythInPlayChanged = new();
        [NonSerialized] public UnityEvent<PlayerParticipant> pauseRequested = new();
        [NonSerialized] public UnityEvent<PlayerParticipant> resumeRequested = new();
        [NonSerialized] public UnityEvent<bool> FaceButtonNorth = new();
        [NonSerialized] public UnityEvent<bool> FaceButtonWest = new();
        [NonSerialized] public UnityEvent<bool> FaceButtonSouth = new();
        [NonSerialized] public UnityEvent<bool> FaceButtonEast = new();

        //Properties
        private bool isAvailableToSwap = true;
        private bool isAvailableToDodge = true;
        [SerializeField] private float swappingCooldown = 1f;
        [SerializeField] private float dodgeCooldown = 2.25f;
        [SerializeField] private PlayerInput playerInput;

        public PlayerInput PlayerInput => playerInput;
    
        //Variables
        //int[] mythsInPlay = { 0, 1 }; //Stores indexes of Myth references in party[] corresponding to each controller 'side'/shoulder button
        // L  R   | mythsInPlay[0] = Left monster = party[mythsInPlay[0]] | opposite for Right monster

        //References
        public Myth MythInPlay
        {
            get => mythInPlay;
            private set
            {
                print("Myth value set: " + value);

                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
            
                if (mythInPlay != null)
                {
                    mythInPlay.ResetAndDisable();

                    position = MythInPlay.transform.position;
                    rotation = MythInPlay.transform.rotation;

                    mythsInReserve.Add(mythInPlay);
                }

                mythInPlay = value;

                if (mythInPlay != null)
                {
                    mythInPlay.gameObject.SetActive(true);
                    mythInPlay.SetAnimatorTrigger("SwapIn");
                
                    if (position != Vector3.zero)
                        MythInPlay.transform.position = position;
                
                    if (rotation != quaternion.identity)
                        MythInPlay.transform.rotation = rotation;
                
                    mythsInReserve.Remove(mythInPlay);
                }

                mythInPlayChanged.Invoke(this);
            }
        }

        private HashSet<Myth> mythsInReserve = new();
        private List<Myth> myths = new();

        // Menu references
        public UIMenuNodeGraph currentMenuGraph;
        public UIMenuNodeGraph currentShoulderMenuGraph; // Alternate menu that you navigate using L/R
        Coroutine cancelCoroutine;
        private Myth mythInPlay;

        private void OnEnable()
        {
            playerParticipantRuntimeSet.Add(this);
        }

        private void OnDisable()
        {
            playerParticipantRuntimeSet.Remove(this);
        }

        private InputActionMap oldInputActionMap;
    
        // TODO: Disabling twice will cause problems
        public void DisablePlayerInput(float timeToWait = 0)
        {
            oldInputActionMap = PlayerInput.currentActionMap;
            PlayerInput.currentActionMap = null;
            if (timeToWait > 0)
                Invoke("EnablePlayerInput", timeToWait);
        }

        public void EnablePlayerInput()
        {
            PlayerInput.currentActionMap = oldInputActionMap;
        }

        /*** In-Game Input events ***/
        #region In-game Input Events

        public void UseAbilityNorth(InputAction.CallbackContext context)
        {
            FaceButtonNorth.Invoke(context.performed);
            UseAbility(context, myth => myth.NorthAbility);
        }

        public void UseAbilityEast(InputAction.CallbackContext context)
        {
            FaceButtonEast.Invoke(context.performed);

            if (!isAvailableToDodge) return;
            if (!context.performed) return;

            if (MythInPlay.MythCommandHandler.LastCommand is MoveCommand moveCommand)
            {
                MythInPlay.MythCommandHandler.PushCommand(new DodgeCommand(moveCommand.input));   // Dodge in the input direction if the left stick is currently in use
                StartDodgeCooldown();
            }
            else
            {
                Vector3 forwardVector = mythInPlay.transform.rotation * Vector3.forward;
                MythInPlay.MythCommandHandler.PushCommand(new DodgeCommand(new Vector2(forwardVector.x, forwardVector.z).normalized));    // Else dodge in direction myth is facing
                StartDodgeCooldown();
            }
        }

        public void UseAbilitySouth(InputAction.CallbackContext context)
        {
            FaceButtonSouth.Invoke(context.performed);
            UseAbility(context, myth => myth.SouthAbility);
        }

        public void UseAbilityWest(InputAction.CallbackContext context)
        {
            FaceButtonWest.Invoke(context.performed);
            UseAbility(context, myth => myth.WestAbility);
        }

        public void Move(InputAction.CallbackContext context)
        {
            // TODO: Not sure if this logic should be here or in MoveCommandReceived
            if (MythInPlay.MythCommandHandler.CurrentCommand is not MoveCommand moveCommand)
                MythInPlay.MythCommandHandler.PushCommand(new MoveCommand(context.ReadValue<Vector2>()));
            else
                moveCommand.input = context.ReadValue<Vector2>();
        }
    
        private void UseAbility(InputAction.CallbackContext context, Func<Myth, SO_Ability> abilityAccessor)
        {
            if (!context.performed) return;

            var ability = abilityAccessor(MythInPlay);

            // TODO: I don't think this is the right place for this check
            if (MythInPlay.Stamina.Value < ability.staminaCost)
            {
                UISFXManager.PlaySound("Ability Denied " + partyIndex);
                return;
            }

            MythInPlay.MythCommandHandler.PushCommand(new AbilityCommand(ability));

            SelectAbility.Invoke(ability);
        }

        public void Pause(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            pauseRequested.Invoke(this);
        }

        public void SwitchLeft(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            SwapCommandInDirection(-1);
        }

        public void SwitchRight(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
        
            SwapCommandInDirection(1);
        }

        #endregion

        private void SwapCommandInDirection(int direction)
        {
            if (!isAvailableToSwap) return;
        
            if (MythInPlay.Health.Value <= 0) return;

            var mythToSwapTo = FindMythToSwapTo(direction);

            if (mythToSwapTo == null) return;
        
            PushSwapCommand(mythToSwapTo);
        }

        private void PushSwapCommand(Myth myth)
        {
            MythInPlay.MythCommandHandler.PushCommand(new SwapCommand(myth, this));
        }

        public void SwapFromCommand(SwapCommand swapCommand)
        {
            MythInPlay = swapCommand.mythToSwapIn;
            StartSwapCooldown();
        }

        public Myth SwapInDirection(int preferredDirection)
        {
            var mythToSwapTo = FindMythToSwapTo(preferredDirection);

            if (mythToSwapTo == null) return null;

            MythInPlay = mythToSwapTo;
        
            StartSwapCooldown();
        
            return mythToSwapTo;
        }

        private Myth FindMythToSwapTo(int preferredDirection)
        {
            int currentMythIndex = myths.IndexOf(mythInPlay);
        
            // Try to swap in specified direction
            int nextIndex = (currentMythIndex + preferredDirection) % myths.Count;
            if (nextIndex < 0) nextIndex = myths.Count - 1;

            if (CanMythBeSwappedTo(nextIndex))
                return myths[nextIndex];
        
            // Try swap in other direction
            nextIndex = (currentMythIndex - preferredDirection) % myths.Count;
            if (nextIndex < 0) nextIndex = myths.Count - 1;

            if (CanMythBeSwappedTo(nextIndex))
                return myths[nextIndex];

            return null;
        }

        private bool CanMythBeSwappedTo(int mythIndex)
        {
            return myths[mythIndex].Health.Value > 0;
        }

        /*** UI Input events ***/
        #region UI Input Events
        public void NavigateUp(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            //print("Player " + partyIndex + " just input UP. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
            currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Up, partyIndex, true);
        }
        public void NavigateDown(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            //print("Player " + partyIndex + " just input DOWN. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
            currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Down, partyIndex, true);
        }

        public void NavigateRight(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            //print("Player " + partyIndex + " just input RIGHT. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
            currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Right, partyIndex, true);

            // If it works it works
            if (!context.performed) return;
            if (currentShoulderMenuGraph == null) return;
            currentShoulderMenuGraph = currentShoulderMenuGraph.ParseNavigation(UIMenuNode.Direction.Right, partyIndex, true);
        }

        public void NavigateLeft(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            //print("Player " + partyIndex + " just input LEFT. Current graph: " + currentMenuGraph.gameObject.name + ", current node: " + currentMenuGraph.playerCurrentNode[partyIndex]);
            currentMenuGraph = currentMenuGraph.ParseNavigation(UIMenuNode.Direction.Left, partyIndex, true);

            // If it works it works
            if (!context.performed) return;
            if (currentShoulderMenuGraph == null) return;
            currentShoulderMenuGraph = currentShoulderMenuGraph.ParseNavigation(UIMenuNode.Direction.Left, partyIndex, true);
        }

        public void NavigateRightShoulder(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (currentShoulderMenuGraph == null) return;
            currentShoulderMenuGraph = currentShoulderMenuGraph.ParseNavigation(UIMenuNode.Direction.Right, partyIndex, true);
        }

        public void NavigateLeftShoulder(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (currentShoulderMenuGraph == null) return;
            currentShoulderMenuGraph = currentShoulderMenuGraph.ParseNavigation(UIMenuNode.Direction.Left, partyIndex, true);
        }

        public void AssignNorth(InputAction.CallbackContext context)
        {
            FaceButtonNorth.Invoke(context.performed);

            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            currentMenuGraph.ParseAction(UIMenuNode.Action.North, partyIndex);
        }
        public void AssignWest(InputAction.CallbackContext context)
        {
            FaceButtonWest.Invoke(context.performed);

            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            currentMenuGraph.ParseAction(UIMenuNode.Action.West, partyIndex);
        }

        public void AssignSouth(InputAction.CallbackContext context)
        {
            FaceButtonSouth.Invoke(context.performed);

            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            currentMenuGraph.ParseAction(UIMenuNode.Action.South, partyIndex);
        }

        public void Submit(InputAction.CallbackContext context)
        {
            //print("Submit " + (context.performed ? "pressed!" : "released!"));

            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            currentMenuGraph.ParseAction(UIMenuNode.Action.Submit, partyIndex);
        }

        public void Cancel(InputAction.CallbackContext context)
        {
            FaceButtonEast.Invoke(context.performed); // Not accurate on keyboard

            //print("Cancel " + (context.performed ? "pressed!" : "released!"));

            if (!context.performed)
            {
                if (cancelCoroutine != null)
                    StopCoroutine(cancelCoroutine);
                return;
            }

            if (currentMenuGraph == null) return;

            currentMenuGraph.ParseAction(UIMenuNode.Action.Cancel, partyIndex);

            if (cancelCoroutine != null)
                StopCoroutine(cancelCoroutine);
            cancelCoroutine = StartCoroutine(HoldCancel(0.65f));
        }

        IEnumerator HoldCancel(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            if (currentMenuGraph != null)
                currentMenuGraph.ParseAction(UIMenuNode.Action.HoldCancel, partyIndex);
        }

        public void StartButton(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (currentMenuGraph == null) return;
            currentMenuGraph.ParseAction(UIMenuNode.Action.Start, partyIndex);
        }

        public void Resume(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
        
            resumeRequested.Invoke(this);
        }

        #endregion

        private void StartSwapCooldown()
        {
            isAvailableToSwap = false;
            Invoke("EndSwapCooldown", swappingCooldown);
        }

        private void StartDodgeCooldown()
        {
            dodgeCooldown = dodgeCooldown - mythInPlay.walkSpeed;
            isAvailableToDodge = false;
            Invoke("EndDodgeCooldown", dodgeCooldown);
        }

        private void EndSwapCooldown()
        {
            isAvailableToSwap = true;
        }

        private void EndDodgeCooldown()
        {
            isAvailableToDodge = true;
        }

        public void Initialise()
        {
            // TODO: This is a quick fix, should find out why this isn't being reset on arena restart
            isAvailableToSwap = true;
            isAvailableToDodge = true;
        
            var allMyths = ParticipantData.partyData[partyIndex].myths;
        
            MythInPlay = allMyths.ElementAtOrDefault(0);
            myths = allMyths.ToList();

            mythsInReserve = Enumerable.ToHashSet(allMyths);

            if (MythInPlay != null)
                mythsInReserve.Remove(MythInPlay);

            foreach (var myth in allMyths)
            {
                myth.died.AddListener(OnMythDied);
            }
        }

        private void OnMythDied(Myth myth)
        {
            StartCoroutine(AutoSwap(myth));
        }

        IEnumerator AutoSwap(Myth myth)
        {
            if (mythInPlay)
            {
                //mythInPlay.SetAnimatorTrigger("Defeat"); // Pointless because myth is set inactive somewhere else, and I have been unsuccessful in sleuthing where
                isAvailableToSwap = false;
                yield return new WaitForSeconds(0.5f); // Potential issues: inputs are still allowed even though 'dead', myth can still be hit even though 'dead'
            }

            var mythToSwapTo = SwapInDirection(1);

            if (mythToSwapTo != null)
            {
                mythToSwapTo.Invulnerability(deathSwapInvulnerabilityTime);
                isAvailableToSwap = true;
            }
        }

        private void Update()
        {
            // TODO: This is some major jank
            mythsInReserve.ForEach(myth =>
            {
                if (myth.Health.Value > 0)
                    myth.Stamina.Update();
            });
        }
    }
}
