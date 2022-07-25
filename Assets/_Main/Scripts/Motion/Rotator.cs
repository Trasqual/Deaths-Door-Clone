using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation = new Vector3(0f, 1f, 0f);
    [SerializeField] private float _speed = 40f;

    private void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime * _speed);
    }
}
