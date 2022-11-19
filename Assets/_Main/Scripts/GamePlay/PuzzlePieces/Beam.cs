using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class Beam : MonoBehaviour
    {
        public Action<RaycastHit> OnBeamHit;
        public Action OnBeamNoHit;

        [SerializeField] Transform _sparkParticle;
        [SerializeField] float _maxDistance = 10f;
        [SerializeField] protected LayerMask _mask;

        LineRenderer _lr;
        protected bool _isActive = true;

        private void Start()
        {
            _lr = GetComponent<LineRenderer>();
            _lr.positionCount = 2;
        }

        protected virtual void Update()
        {
            if (_isActive)
            {
                CastBeam();
            }
        }

        public virtual void Activate()
        {
            _isActive = true;
        }

        public virtual void DeActivate()
        {
            _isActive = false;
            UpdateBeam(new Vector3[] { transform.position, transform.position });
        }

        private void CastBeam()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _mask, QueryTriggerInteraction.Ignore))
            {
                OnBeamHit?.Invoke(hit);
                UpdateBeam(new Vector3[] { transform.position, hit.point });
            }
            else
            {
                OnBeamNoHit?.Invoke();
                UpdateBeam(new Vector3[] { transform.position, transform.position + transform.forward * _maxDistance });
            }
        }

        public void UpdateBeam(Vector3[] positions)
        {
            _lr.SetPositions(positions);
            _lr.widthMultiplier = Random.Range(1f, 1.2f);
            _sparkParticle.position = _lr.GetPosition(1);
            _sparkParticle.LookAt(_lr.GetPosition(0));
        }

        public void SetMaxDistance(float maxDistance)
        {
            _maxDistance = maxDistance;
        }
    }
}