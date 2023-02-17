namespace EffectSystem.EffectSubtypes
{
    public class DebuffCleanse : Effect
    {
        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            effects.RemoveEffects(effect => effect.isDebuff);
        }
    }
}