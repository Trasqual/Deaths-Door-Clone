using System.Collections;
using UnityEngine;

public class Invulnerable : InvulnerableBase
{
    public override void SetInvulnerable()
    {
        StopInvulnerabilityCoroutine();
        IsActive = true;
    }

    public override void SetVulnerable()
    {
        IsActive = false;
    }

    public override IEnumerator SetInvulnerableForDuration(float duration)
    {
        IsActive = true;
        yield return new WaitForSeconds(duration);
        IsActive = false;
    }

    public override void StopInvulnerabilityCoroutine()
    {
        if (_invulnerabilityCoroutine != null)
        {
            StopCoroutine(_invulnerabilityCoroutine);
        }
    }

    public override void InvulnerableForDuration(float duration)
    {
        StopInvulnerabilityCoroutine();
        _invulnerabilityCoroutine = SetInvulnerableForDuration(duration);
        StartCoroutine(_invulnerabilityCoroutine);
        IsActive = true;
    }
}