using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Puzzles
{
    [SelectionBase]
    public class FallingFloor : MonoBehaviour
    {
        [SerializeField] private float timeBeforeFall = 0.5f;

        [SerializeField] private Transform _visual;
        [SerializeField] private BoxCollider _col;

        private Vector3 _visualStartPos;
        private bool _isActive;

        private void Start()
        {
            _visualStartPos = _visual.localPosition;
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
            s.Append(_visual.DOShakePosition(timeBeforeFall, 0.05f, 10, 60).OnComplete(() => _col.enabled = false));
            s.Append(_visual.DOMoveY(-30f, 3f).SetRelative());
            s.Join(DOVirtual.Float(0f, 1f, 1f, (x) => _visual.GetComponent<MeshRenderer>().material.SetFloat("_DissolveValue", x)));
            s.AppendInterval(2.5f);
            s.OnComplete(() =>
            {
                ResetFloor();
            });
        }

        private void ResetFloor()
        {
            _visual.localPosition = _visualStartPos;
            DOVirtual.Float(1f, 0f, 1f, (x) => _visual.GetComponent<MeshRenderer>().material.SetFloat("_DissolveValue", x));
            _col.enabled = true;
            _isActive = false;
        }
    }
}

