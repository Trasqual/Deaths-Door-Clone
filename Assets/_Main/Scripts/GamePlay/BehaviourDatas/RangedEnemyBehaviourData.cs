using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class RangedEnemyBehaviourData : EnemyBehaviourData
{
    [TitleGroup("RangedAttack")]
    public float CastSpeed = 1f;
}
