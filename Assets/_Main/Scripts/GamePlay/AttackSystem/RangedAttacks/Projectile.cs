using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class Projectile : MonoBehaviour, IDamageDealer
    {
        [SerializeField] float projectileSpeed = 5f;
        [SerializeField] float baseDamage = 1f;
        private float _dmgMultiplier = 1f;
        private float _damage => baseDamage * _dmgMultiplier;
        private DamageDealerType _damageDealerType;

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
            transform.SetParent(collision.transform);
            if (collision.collider.TryGetComponent(out IDamagable damagable))
            {
                DealDamage(Mathf.RoundToInt(_damage), damagable, _damageDealerType);
            }
        }

        public void Init(float dmgMultiplier, DamageDealerType damageDealerType)
        {
            SetDmgMultiplier(dmgMultiplier);
            SetDamageDealerType(damageDealerType);
        }

        public void SetDamageDealerType(DamageDealerType damageDealerType)
        {
            _damageDealerType = damageDealerType;
        }

        public void SetDmgMultiplier(float amount)
        {
            _dmgMultiplier = amount;
        }

        IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

        public void DealDamage(int damage, IDamagable damagable, DamageDealerType damageDealerType)
        {
            damagable.TakeDamage(damage, damageDealerType);
        }
    }
}
