using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.HealthSystem
{
    public abstract class InvulnerableBase : MonoBehaviour
    {
        protected IEnumerator InvulnerabilityCoroutine;

        public bool IsActive { protected set; get; } = false;

        public abstract void SetInvulnerable();
        public abstract void SetVulnerable();
        public abstract IEnumerator SetInvulnerableForDuration(float duration);
        public abstract void StopInvulnerabilityCoroutine();
        public abstract void InvulnerableForDuration(float duration);
    }
}