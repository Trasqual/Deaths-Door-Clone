using System.Collections;
using UnityEngine;

public abstract class InvulnerableBase : MonoBehaviour
{
    protected IEnumerator _invulnerabilityCoroutine;
    
    public bool IsActive { protected set; get; } = false;

    public abstract void SetInvulnerable();
    public abstract void SetVulnerable();
    public abstract IEnumerator SetInvulnerableForDuration(float duration);
    public abstract void StopInvulnerabilityCoroutine();
    public abstract void InvulnerableForDuration(float duration);
}