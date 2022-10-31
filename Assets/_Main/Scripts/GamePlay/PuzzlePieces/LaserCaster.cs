using UnityEngine;

public class LaserCaster : MonoBehaviour
{
    [SerializeField] protected float _distance = 10f;
    [SerializeField] protected LaserBeam _laserBeam;
    [SerializeField] protected LayerMask _mask;

    protected LaserBeam _spawnedBeam;
    protected bool isActive = true;

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

    public virtual void CastLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, _distance, _mask, QueryTriggerInteraction.Ignore);
        if (hit.transform != null)
        {
            _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, (hit.point - transform.forward * 0.1f) });
        }
        else
        {
            _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, transform.position + transform.forward * _distance });
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void DeActivate()
    {
        isActive = false;
        _spawnedBeam.UpdateBeam(new Vector3[] { transform.position, transform.position });
    }
}
