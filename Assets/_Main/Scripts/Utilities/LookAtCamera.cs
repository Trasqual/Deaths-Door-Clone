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
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 50f);
        var angles = transform.eulerAngles;
        angles.z = 0f;
        transform.eulerAngles = angles;
    }
}
