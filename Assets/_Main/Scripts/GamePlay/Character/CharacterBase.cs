using System;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public Action<AttackBase> OnSelectedMeleeAttackChanged;
    public Action<AttackBase> OnSelectedRangedAttackChanged;

    protected StateMachine stateMachine;
    public AttackBase SelectedRangedAttack { get; protected set; }
    public AttackBase SelectedMeleeAttack { get; protected set; }

    protected virtual void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }
    protected virtual void TakeDamage(int i) { }
    protected virtual void Die() { }
}
