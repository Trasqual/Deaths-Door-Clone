using Sirenix.OdinInspector;
using UnityEngine;

public class BehaviourData : ScriptableObject
{
    [TitleGroup("Base")]
    public string Name  = "";

    [TitleGroup("Locomotion")]
    public float MovementSpeed  = 5F;
    public float RotationSpeed = 5F;
    public float DodgeSpeed  = 5F;

    [TitleGroup("Health")]
    public int Health = 4;
    public float DamageTakenDuration = 0.5f;

    [TitleGroup("Attack")]
    public float AttackRange = 1.5f;
}
