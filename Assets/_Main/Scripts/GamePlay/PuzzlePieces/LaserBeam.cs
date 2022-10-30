using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private Transform _sparkParticle;
    private LineRenderer _lr;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.positionCount = 2;
    }

    public void UpdateBeam(Vector3[] positions)
    {
        _lr.SetPositions(positions);
        _lr.widthMultiplier = Random.Range(1f, 1.2f);
        _sparkParticle.position = _lr.GetPosition(1);
        _sparkParticle.LookAt(_lr.GetPosition(0));
    }
}
