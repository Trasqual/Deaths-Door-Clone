using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Puzzles
{
    [SelectionBase]
    public class FallingFloor : MonoBehaviour
    {
        [SerializeField] private float _timeBeforeFall = 1f;
        [SerializeField] private float _fallDuration = 3f;
        [SerializeField] private float _resetTime = 2f;
        [SerializeField] private float _shakeIntensity = 0.05f;
        [SerializeField] private float _fallDistance = 30f;
        [SerializeField] private float _dissolveTime = 1f;

        [SerializeField] private Transform _visual;
        private MeshRenderer _visualRenderer;
        [SerializeField] private BoxCollider _col;

        private Vector3 _visualStartPos;
        private bool _isActive;

        private void Start()
        {
            _visualStartPos = _visual.localPosition;
            _visualRenderer = _visual.GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive) return;
            if (other.TryGetComponent(out CharacterController player))
            {
                ActivateFloor();
            }
        }

        private void ActivateFloor()
        {
            _isActive = true;

            Sequence s = DOTween.Sequence();
            s.Append(_visual.DOShakePosition(_timeBeforeFall, _shakeIntensity, 10, 60).OnComplete(() => _col.enabled = false));
            s.Append(_visual.DOMoveY(-_fallDistance, _fallDuration).SetRelative());
            _visualRenderer.material.SetFloat("_EdgeThickness", 0f);
            s.Join(DOVirtual.Float(0f, 1f, _dissolveTime, (x) => _visualRenderer.material.SetFloat("_DissolveValue", x)));
            s.AppendInterval(_resetTime);
            s.OnComplete(() =>
            {
                ResetFloor();
            });
        }

        private void ResetFloor()
        {
            _visual.localPosition = _visualStartPos;
            _visualRenderer.material.SetFloat("_EdgeThickness", 0.02f);
            DOVirtual.Float(1f, 0f, _dissolveTime, (x) => _visualRenderer.material.SetFloat("_DissolveValue", x)).OnComplete(() => _visualRenderer.material.SetFloat("_EdgeThickness", 0f));
            _col.enabled = true;
            _isActive = false;
        }
    }
}

