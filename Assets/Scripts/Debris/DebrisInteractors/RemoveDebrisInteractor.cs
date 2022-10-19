namespace Debris.DebrisInteractors
{
    public class RemoveDebrisInteractor : DebrisInteractor
    {
        internal override void OnDebrisEnter(Debris debris)
        {
            base.OnDebrisEnter(debris);
            
            debris.RemoveDebris();
        }
    }
}