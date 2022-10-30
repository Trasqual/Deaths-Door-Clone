using System;
using UnityEngine;

public abstract class DetectorBase<T> : MonoBehaviour
{
    public Action<T> OnTargetFound;
    public Action OnTargetLost;
    public abstract void Detect(T target);
}
