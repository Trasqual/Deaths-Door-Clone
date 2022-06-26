using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 5f;
        [SerializeField] float baseDamage = 1f;
        float dmgMultiplier = 1f;
        float damage => baseDamage * dmgMultiplier;

        Rigidbody rb;
        public Collider Col => GetComponent<Collider>();

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            StartCoroutine(DestroySelf());
        }

        private void OnCollisionEnter(Collision collision)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;
            Col.enabled = false;
        }

        public void SetDmgMultiplier(float amount)
        {
            dmgMultiplier = amount;
        }

        IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}
