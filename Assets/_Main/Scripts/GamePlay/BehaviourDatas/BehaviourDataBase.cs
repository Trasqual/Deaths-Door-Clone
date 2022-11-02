using Sirenix.OdinInspector;
using UnityEngine;

public class BehaviourData : ScriptableObject
{
    [TitleGroup("Base")]
    public string Name  = "";

    [TitleGroup("Locomotion")]
    public float MovementSpeed  = 5F;
    public float RotationSpeed = 5F;

    [TitleGroup("Dodge")]
    public float DodgeSpeedMultiplier  = 2F;
    public float DodgeDuration  = 0.5F;
    public float DodgeCD  = 2F;

    [TitleGroup("Health")]
    public int Health = 4;
    public float DamageTakenStateDuration = 0.5f;
    public float InvulnerabilityDurationAfterTakingDamage = 1f;

    [TitleGroup("Attack")]
    public float AttackRange = 1.5f;
}
