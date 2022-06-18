using UnityEngine;

public class Shooter : MonoBehaviour
{
    Collider col;

    private void Awake()
    {
        col = GetComponentInParent<Collider>();
    }

    public void Shoot(Projectile projectilePrefab, float dmgMultiplier)
    {
        var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.SetDmgMultiplier(dmgMultiplier);
        Physics.IgnoreCollision(col, projectile.Col);
    }
}
