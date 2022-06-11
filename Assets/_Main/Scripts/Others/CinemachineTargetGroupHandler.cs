using Cinemachine;
using UnityEngine;

public class CinemachineTargetGroupHandler : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup targetGroup;

    public void ChangeTargetWeight(int targetIndex, float newWeight)
    {
        targetGroup.m_Targets[targetIndex].weight = newWeight;
    }
}
