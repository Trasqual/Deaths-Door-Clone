using System;

namespace _Main.Scripts.Utilities
{
    public class Enums
    {
        public static bool CompareEnums(DamageDealerType effector, DamageDealerType effected)
        {
            int commonBitmask = (int)effector & (int)effected;

            foreach (DamageDealerType currentEnum in Enum.GetValues(typeof(DamageDealerType)))
            {
                if ((commonBitmask & (int)currentEnum) != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}