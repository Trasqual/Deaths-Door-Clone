using UnityEngine;

public class Shooter : MonoBehaviour
{
    public void Shoot(Projectile projectilePrefab, float dmgMultiplier)
    {
        var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.SetDmgMultiplier(dmgMultiplier);
    }
}
