using UnityEngine;

public class LaserCaster : MonoBehaviour
{
    [SerializeField] private float distance = 10f;
    [SerializeField] private LaserBeam _laserBeam;

    LaserBeam _spawnedBeam;
    bool isActive = true;

    private void Start()
    {
        _spawnedBeam = Instantiate(_laserBeam, transform.position, transform.rotation, transform);
    }

    private void Update()
    {
        if (isActive)
        {
            CastLaser();
        }
    }

    public void CastLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, distance);
        if (hit.transform != null)
        {
            _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, (hit.point - transform.forward * 0.1f) });
        }
        else
        {
            _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, transform.position + transform.forward * distance });
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void DeActivate()
    {
        isActive = false;
    }
}
