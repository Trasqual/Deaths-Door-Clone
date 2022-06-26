using System;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class Transition
    {
        public Type To;
        public Func<bool> Condition;
        public Func<bool> Override;

        public Transition(Type to, Func<bool> condition, Func<bool> shouldOverride)
        {
            To = to;
            Condition = condition;
            Override = shouldOverride;
        }
    }
}