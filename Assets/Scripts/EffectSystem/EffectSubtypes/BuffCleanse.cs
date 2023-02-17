namespace EffectSystem.EffectSubtypes
{
    public class BuffCleanse : Effect
    {
        public override void ApplyEffect()
        {
            base.ApplyEffect();
            
            effects.RemoveEffects(effect => !effect.isDebuff);
        }
    }
}