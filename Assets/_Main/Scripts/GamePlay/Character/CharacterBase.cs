using _Main.Scripts.GamePlay.StateMachine;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected StateMachine stateMachine;

    protected virtual void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    protected virtual void Die() { }
}
