using Cinemachine;
using System.Collections;
using UnityEngine;

public class CinemachineTargetGroupHandler : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] AimAction aimHandler;
    [SerializeField] float aimWeightChangeDuration = 2f;

    IEnumerator aimWeightCo;

    private void OnEnable()
    {
        if (aimHandler)
        {
            aimHandler.OnActionStarted += SetAimingWeights;
            aimHandler.OnActionEnded += ResetAimingWeights;
        }
    }

    public void SetAimingWeights()
    {
        if (aimWeightCo != null)
        {
            StopCoroutine(aimWeightCo);
        }
        aimWeightCo = ChangeWeightsCo(new int[2] { 0, 1 }, new float[2] { .75f, 1f }, aimWeightChangeDuration);
        StartCoroutine(aimWeightCo);
    }

    public void ResetAimingWeights()
    {
        if (aimWeightCo != null)
        {
            StopCoroutine(aimWeightCo);
        }
        aimWeightCo = ChangeWeightsCo(new int[2] { 0, 1 }, new float[2] { .75f, 0.5f }, aimWeightChangeDuration);
        StartCoroutine(aimWeightCo);
    }

    IEnumerator ChangeWeightsCo(int[] targetsToChange, float[] newWeights, float duration)
    {
        var t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            for (int i = 0; i < targetsToChange.Length; i++)
            {
                targetGroup.m_Targets[targetsToChange[i]].weight = Mathf.Lerp(targetGroup.m_Targets[targetsToChange[i]].weight, newWeights[i], t / duration);
            }
            yield return null;
        }
    }
}
