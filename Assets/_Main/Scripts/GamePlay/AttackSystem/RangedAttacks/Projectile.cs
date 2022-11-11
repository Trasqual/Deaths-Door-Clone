using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float maxTravelDistance = 15f;
        [SerializeField] float projectileSpeed = 5f;
        private float _damage;
        private DamageDealerType _damageDealerType;
        private Vector3 startPos;
        private IDamageable _caster;

        Rigidbody rb;
        public Collider Col => GetComponent<Collider>();

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            startPos = transform.position;
        }

        private void FixedUpdate()
        {
            if(Vector3.Distance(transform.position, startPos) >= maxTravelDistance)
            {
                StartCoroutine(DestroySelf(0f));
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;
            Col.enabled = false;
            if(collision.collider.TryGetComponent(out ProjectileBoneStickingHandler boneStickHandler))
            {
                var newParent = boneStickHandler.GetClosestBone(collision.contacts[0].point);
                transform.SetParent(newParent);
                transform.position = newParent.position;
            }
            else
            {
                transform.SetParent(collision.transform);
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

        public void Init(float damage, DamageDealerType damageDealerType, IDamageable caster)
        {
            SetDamage(damage);
            SetDamageDealerType(damageDealerType);
            SetCaster(caster);
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }

        public void SetDamageDealerType(DamageDealerType damageDealerType)
        {
            _damageDealerType = damageDealerType;
        }

        public void SetCaster(IDamageable caster)
        {
            _caster = caster;
        }

        IEnumerator DestroySelf(float duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}
