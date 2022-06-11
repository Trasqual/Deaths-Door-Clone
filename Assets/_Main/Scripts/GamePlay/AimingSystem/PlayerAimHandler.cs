using UnityEngine;

public class PlayerAimHandler : AimHandler
{
    [SerializeField] private Transform indicator;

    [SerializeField] private CinemachineTargetGroupHandler camTargetGroup;

    protected override void OnAimStarted()
    {
        base.OnAimStarted();
        indicator.gameObject.SetActive(true);
        camTargetGroup.ChangeTargetWeight(0, 0.75f);
        camTargetGroup.ChangeTargetWeight(1, 1);
    }

    protected override void OnAimEnded()
    {
        base.OnAimEnded();
        indicator.gameObject.SetActive(false);
        camTargetGroup.ChangeTargetWeight(0, 1);
        camTargetGroup.ChangeTargetWeight(1, 0);
        indicator.position = transform.position;
    }

    protected override void ProcessAimRotation()
    {
        base.ProcessAimRotation();
        SetIndicator();
    }

    private void SetIndicator()
    {
        indicator.position = transform.forward * 7f;
        indicator.rotation = transform.rotation;
    }
}
