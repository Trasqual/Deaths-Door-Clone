using System;

namespace _Main.Scripts.Utilities
{
    public class Enums
    {
        public static bool CompareEnums<T>(T effector, T effected) where T : IConvertible
        {
            int commonBitmask = Convert.ToInt32(effector) & Convert.ToInt32(effected);

            foreach (T currentEnum in Enum.GetValues(typeof(T)))
            {
                if ((commonBitmask & Convert.ToInt32(currentEnum)) != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}