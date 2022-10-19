namespace Debris.DebrisInteractors
{
    public class AgilityDebuffDebrisInteractor : ApplyEffectDebrisInteractor
    {
        protected override void ApplyEffect()
        {
            base.ApplyEffect();
            
            effects.AgilityDebuff();
        }

        protected override void RemoveEffect()
        {
            base.RemoveEffect();
            
            effects.RemoveAgilityDebuff();
        }
    }
}