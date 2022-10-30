using _Main.Scripts.GamePlay.Indicators.ActionIndicator;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Indicators.AimingIndicator
{
    public class AimActionIndicator : ActionIndicatorBase
    {
        [SerializeField] private GameObject _indicatorVisual;
        [SerializeField] private float _indicatorDistance = 7f;
        [SerializeField] private float _indicatorMoveSpeed = 10f;

        private Tweener _resetTween;

        protected override void Activate()
        {
            _indicatorVisual.SetActive(true);
            if (_resetTween != null) _resetTween.Kill();
        }

        protected override void Deactivate()
        {
            _indicatorVisual.SetActive(false);
            transform.rotation = Quaternion.identity;
            _resetTween = transform.DOLocalMove(Vector3.zero, 0.5f);
        }

        private void Update()
        {
            if (_indicatorVisual.activeSelf)
            {
                DoOnActionPerformed();
            }
        }

        protected override void DoOnActionPerformed()
        {
            SetPositionAndRotation();
        }

        private void SetPositionAndRotation()
        {
            var target = transform.parent.position + (transform.parent.forward * _indicatorDistance);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * _indicatorMoveSpeed);
            transform.rotation = transform.parent.rotation;
        }
    }
}
