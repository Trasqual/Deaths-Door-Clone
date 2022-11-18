using _Main.Scripts.GamePlay.DetectionSystem;
using _Main.Scripts.GamePlay.HealthSystem;
using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class ArrowProjectile : Projectile
    {
        protected Vector3 startPos;

        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            startPos = transform.position;
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, startPos) >= maxTravelDistance)
            {
                StartCoroutine(DestroySelf(0f));
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;
            Col.enabled = false;
            if (collision.collider.TryGetComponent(out ProjectileBoneStickingHandler boneStickHandler))
            {
                var newParent = boneStickHandler.GetClosestBone(collision.contacts[0].point);
                transform.SetParent(newParent);
                transform.position = newParent.position;
            }
            else
            {
                rb.isKinematic = true;
            }
            if (collision.collider.TryGetComponent(out IDamageable damagable))
            {
                damagable.TakeDamage(Mathf.RoundToInt(_damage), _damageDealerType);
            }
            var detector = collision.collider.GetComponentInChildren<DetectorBase<IDamageable>>();
            if (detector != null)
            {
                detector.Detect(_caster);
            }
            StartCoroutine(DestroySelf(3f));
        }

        IEnumerator DestroySelf(float duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }


}