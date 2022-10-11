using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected StateMachine stateMachine;
    public AttackBase SelectedRangedAttack { get; protected set; }

    protected virtual void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }
    protected virtual void TakeDamage(int i) { }
    protected virtual void Die() { }
}
