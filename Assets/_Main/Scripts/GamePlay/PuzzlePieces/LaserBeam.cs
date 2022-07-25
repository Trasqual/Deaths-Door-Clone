using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] Transform sparkParticle;
    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    public void UpdateBeam(Vector3[] positions)
    {
        lr.SetPositions(positions);
        lr.widthMultiplier = Random.Range(1f, 1.2f);
        sparkParticle.position = lr.GetPosition(1);
        sparkParticle.LookAt(lr.GetPosition(0));
    }
}
