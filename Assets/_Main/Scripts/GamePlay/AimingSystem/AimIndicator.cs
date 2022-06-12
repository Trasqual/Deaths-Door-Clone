using DG.Tweening;
using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    [SerializeField] GameObject indicatorVisual;
    [SerializeField] float indicatorDistance = 7f;
    [SerializeField] float indicatorMoveSpeed = 10f;

    AimHandler aimHandler;

    Tweener resetTween;

    private void Start()
    {
        aimHandler = GetComponentInParent<AimHandler>();

        if (aimHandler)
        {
            aimHandler.OnAimActionStarted += Activate;
            aimHandler.OnAimActionEnded += Deactivate;
            aimHandler.OnAimActionPerformed += SetPositionAndRotation;
        }
    }

    public void Activate()
    {
        indicatorVisual.SetActive(true);
        if (resetTween != null) resetTween.Kill();
    }

    public void Deactivate()
    {
        indicatorVisual.SetActive(false);
        transform.rotation = Quaternion.identity;
        resetTween = transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    public void SetPositionAndRotation()
    {
        var target = aimHandler.transform.position + (aimHandler.transform.forward * indicatorDistance);
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * indicatorMoveSpeed);
        transform.rotation = aimHandler.transform.rotation;
    }
}
