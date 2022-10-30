using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoneStickingHandler : MonoBehaviour
{
    [SerializeField] private Transform[] _bones;

    public Transform GetClosestBone(Vector3 pointOfImpact)
    {
        Transform closestBone = null;
        var closestDist = float.MaxValue;

        for (int i = 0; i < _bones.Length; i++)
        {
            var mag = (pointOfImpact - _bones[i].position).sqrMagnitude;
            if (mag < closestDist)
            {
                closestDist = mag;
                closestBone = _bones[i];
            }
        }
        return closestBone;
    }
}
