using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Puzzles
{
    [SelectionBase]
    public class FallingFloor : MonoBehaviour
    {
        [SerializeField] private float timeBeforeFall = 1f;
        [SerializeField] private float fallDuration = 3f;
        [SerializeField] private float resetTime = 2f;
        [SerializeField] private float shakeIntensity = 0.05f;
        [SerializeField] private float fallDistance = 30f;
        [SerializeField] private float dissolveTime = 1f;

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
            s.Append(_visual.DOShakePosition(timeBeforeFall, shakeIntensity, 10, 60).OnComplete(() => _col.enabled = false));
            s.Append(_visual.DOMoveY(-fallDistance, fallDuration).SetRelative());
            _visualRenderer.material.SetFloat("_EdgeThickness", 0f);
            s.Join(DOVirtual.Float(0f, 1f, dissolveTime, (x) => _visualRenderer.material.SetFloat("_DissolveValue", x)));
            s.AppendInterval(resetTime);
            s.OnComplete(() =>
            {
                ResetFloor();
            });
        }

        private void ResetFloor()
        {
            _visual.localPosition = _visualStartPos;
            _visualRenderer.material.SetFloat("_EdgeThickness", 0.02f);
            DOVirtual.Float(1f, 0f, dissolveTime, (x) => _visualRenderer.material.SetFloat("_DissolveValue", x)).OnComplete(() => _visualRenderer.material.SetFloat("_EdgeThickness", 0f));
            _col.enabled = true;
            _isActive = false;
        }
    }
}

