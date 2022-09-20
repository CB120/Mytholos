using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGameAbility : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI abilityNameTMP;
    [SerializeField] private Image elementIcon;
    [SerializeField] private TextMeshProUGUI staminaCostTMP;
    [SerializeField] private Color[] elementColours;

    public void UpdateUI(string abilityName, E_Element element, float staminaCost)
    {
        abilityNameTMP.text = abilityName;
        elementIcon.color = elementColours[(int)element]; // TODO: Create a scriptiable object for commonly used asset resources (in this case, symbols/colours for each element)
        //staminaCostTMP.text = Mathf.RoundToInt(staminaCost * 100.0f) + "%"; // Assumes that stamina costs are passed in as a float ranging between 0 and 1
        staminaCostTMP.text = Mathf.RoundToInt(staminaCost) + ""; // For sprint 2, we'll just show the damage
    }

    public void AnimateSelectedAbility()
    {
        UIGameAbility effect = Instantiate<GameObject>(gameObject, transform.parent).GetComponent<UIGameAbility>();
        effect.StartCoroutine(effect.AnimateThisAbilityThenDie());
    }

    IEnumerator AnimateThisAbilityThenDie()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        float timer = 0.0f;

        while (timer < 0.25f)
        {
            float newScale = Time.deltaTime * 0.35f + rectTransform.localScale.x;
            rectTransform.localScale = new Vector3(newScale, newScale, newScale);
            canvasGroup.alpha = 1 - (timer / 0.35f);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }

        Destroy(gameObject);
    }

    // TODO: Greying out ability if insufficient stamina, and placing a listener somewhere to call a function here that (animates and) un-greys out this ability when ready
    // Also, a lister to grey out the ability if stamina drops below required?
}
