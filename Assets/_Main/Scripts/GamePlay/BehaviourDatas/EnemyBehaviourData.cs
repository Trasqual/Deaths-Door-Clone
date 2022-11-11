using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class EnemyBehaviourData : BehaviourBaseData
{
    [TitleGroup("Locomotion")]
    public float MovementSpeed  = 5F;
    public float RotationSpeed = 5F;

    [TitleGroup("Health")]
    public int Health = 4;
}