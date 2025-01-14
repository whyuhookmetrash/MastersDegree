public interface IModifier
{
    void SetModifier(Actor actor);
    void RemoveModifier(Actor actor);
}

namespace Modifiers
{

    public class MaxHP : IModifier
    {
        private int value;
        public MaxHP(int _value)
        {
            value = _value;
        }
        public void SetModifier(Actor actor)
        {
            actor.maxHP += value;
            actor.currentHP += value;
        }
        public void RemoveModifier(Actor actor)
        {
            actor.maxHP -= value;
            actor.currentHP -= value;
        }

    }

    public class DamageValue : IModifier
    {
        private int value;
        public DamageValue(int _value)
        {
            value = _value;
        }
        public void SetModifier(Actor actor)
        {
            actor.currentDamageInfo.damageValue += value;
        }
        public void RemoveModifier(Actor actor)
        {
            actor.currentDamageInfo.damageValue -= value;
        }
    }
}
