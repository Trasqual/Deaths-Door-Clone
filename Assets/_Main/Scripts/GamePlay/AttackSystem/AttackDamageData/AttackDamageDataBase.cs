using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class AttackDamageDataBase : ScriptableObject
    {
        public float attackRange = 2;
        public int damage = 1;
        public DamageDealerType dmgDealerType;
    }
}