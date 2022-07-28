using MainMode.Items;
using MainMode.Effects;

namespace MainMode
{
    public interface IAddEffects
    {
        public void AddEffects(MovementEffect floats,float timeActive, EffectType type = EffectType.None);
        public bool AddEffects(ITransport movement, float timeActive);
    }
}

