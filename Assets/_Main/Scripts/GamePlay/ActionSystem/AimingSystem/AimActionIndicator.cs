using DG.Tweening;
using UnityEngine;

public class AimActionIndicator : ActionIndicatorBase
{
    [SerializeField] GameObject indicatorVisual;
    [SerializeField] float indicatorDistance = 7f;
    [SerializeField] float indicatorMoveSpeed = 10f;

    Tweener resetTween;

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

    protected override void DoOnActionPerformed()
    {
        SetPositionAndRotation();
    }

    private void SetPositionAndRotation()
    {
        var target = action.transform.position + (action.transform.forward * indicatorDistance);
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * indicatorMoveSpeed);
        transform.rotation = action.transform.rotation;
    }
}
