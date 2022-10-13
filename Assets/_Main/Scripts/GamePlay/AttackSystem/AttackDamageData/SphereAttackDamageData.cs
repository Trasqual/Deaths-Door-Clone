using UnityEngine;

[CreateAssetMenu(menuName = "AttackDamageData/SphereAttackDamageData")]
public class SphereAttackDamageData : AttackDamageDataBase
{
    public float radius;
    public float range;
    public int damage;
    public DamageDealerType dmgDealerType;
}
