using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation =  cam.transform.rotation;
    }
}
