using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        var dir = (transform.position - cam.transform.position);
        transform.rotation = Quaternion.LookRotation(dir);
        var angles = transform.eulerAngles;
        angles.y = 0f;
        angles.z = 0f;
        transform.eulerAngles = angles;
    }
}
