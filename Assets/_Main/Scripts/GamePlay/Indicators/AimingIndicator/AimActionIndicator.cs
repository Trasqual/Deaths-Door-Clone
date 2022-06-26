using _Main.Scripts.GamePlay.Indicators.ActionIndicator;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Indicators.AimingIndicator
{
    public class AimActionIndicator : ActionIndicatorBase
    {
        [SerializeField] private GameObject indicatorVisual;
        [SerializeField] private float indicatorDistance = 7f;
        [SerializeField] private float indicatorMoveSpeed = 10f;

        private Tweener resetTween;

        protected override void Activate()
        {
            indicatorVisual.SetActive(true);
            if (resetTween != null) resetTween.Kill();
        }

        protected override void Deactivate()
        {
            indicatorVisual.SetActive(false);
            transform.rotation = Quaternion.identity;
            resetTween = transform.DOLocalMove(Vector3.zero, 0.5f);
        }

        private void Update()
        {
            if (indicatorVisual.activeSelf)
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
            var target = transform.parent.position + (transform.parent.forward * indicatorDistance);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * indicatorMoveSpeed);
            transform.rotation = transform.parent.rotation;
        }
    }
}
