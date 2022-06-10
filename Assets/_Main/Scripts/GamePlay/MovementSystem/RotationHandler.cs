using UnityEngine;

public class RotationHandler : MonoBehaviour
{
    public void ProcessRotation(Vector3 direction, float rotationSpeed)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }
    }
}
