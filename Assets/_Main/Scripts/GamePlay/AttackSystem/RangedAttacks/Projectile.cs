using System.Collections;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem.RangedAttacks
{
    public class Projectile : MonoBehaviour, IDamageDealer
    {
        [SerializeField] private float _maxTravelDistance = 15f;
        [SerializeField] private float _projectileSpeed = 5f;
        [SerializeField] private float _baseDamage = 1f;
        private float _dmgMultiplier = 1f;
        private float _damage => _baseDamage * _dmgMultiplier;
        private DamageDealerType _damageDealerType;
        private Vector3 _startPos;
        private IDamageable _caster;

        private Rigidbody _rb;
        public Collider Col => GetComponent<Collider>();

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.AddForce(transform.forward * _projectileSpeed, ForceMode.Impulse);
            _startPos = transform.position;
        }

        private void FixedUpdate()
        {
            if(Vector3.Distance(transform.position, _startPos) >= _maxTravelDistance)
            {
                StartCoroutine(DestroySelf(0f));
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            _rb.isKinematic = true;
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
                DealDamage(Mathf.RoundToInt(_damage), damagable, _damageDealerType);
            }
            var detector = collision.collider.GetComponentInChildren<DetectorBase<IDamageable>>();
            if (detector != null)
            {
                detector.Detect(_caster);
            }
            StartCoroutine(DestroySelf(3f));
        }

        public void Init(float dmgMultiplier, DamageDealerType damageDealerType, IDamageable caster)
        {
            SetDmgMultiplier(dmgMultiplier);
            SetDamageDealerType(damageDealerType);
            SetCaster(caster);
        }

        public void SetDamageDealerType(DamageDealerType damageDealerType)
        {
            _damageDealerType = damageDealerType;
        }

        public void SetDmgMultiplier(float amount)
        {
            _dmgMultiplier = amount;
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

        public void DealDamage(int damage, IDamageable damagable, DamageDealerType damageDealerType)
        {
            damagable.TakeDamage(damage, damageDealerType);
        }
    }
}
