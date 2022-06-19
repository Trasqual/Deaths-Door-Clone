using UnityEngine;

public class BowAttack : AttackBase
{
    [Header("Visuals")]
    [SerializeField] private GameObject bow;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Projectile chargedProjectilePrefab;

    [Header("Shooters")]
    [SerializeField] private Shooter shooter;

    [Header("Attack Params")]
    [SerializeField] private float initialChargeDelay = 0.5f;
    [SerializeField] private float maxChargeTime = 3f;
    [SerializeField] private float minDmgMultiplier = 1f;
    [SerializeField] private float maxDmgMultiplier = 2f;
    private float chargeDelayDuration;
    private float chargeDuration;
    private float dmgMultiplier;

    private bool isActive;

    public override void DoOnActionStart()
    {
        bow.SetActive(true);
        isActive = true;
    }

    public override void DoOnActionEnd()
    {
        isActive = false;
        Shoot();
        chargeDelayDuration = 0f;
        chargeDuration = 0f;
        dmgMultiplier = 1f;
        bow.SetActive(false);
    }

    public override void DoOnActionCanceled()
    {
        isActive = false;
        chargeDelayDuration = 0f;
        chargeDuration = 0f;
        dmgMultiplier = 1f;
        bow.SetActive(false);
    }

    private void Update()
    {
        if (isActive)
        {
            ChargeAttack();
        }
    }

    private void ChargeAttack()
    {
        if (chargeDelayDuration <= initialChargeDelay)
        {
            chargeDelayDuration += Time.deltaTime;
        }
        else
        {
            if (chargeDuration <= maxChargeTime)
            {
                chargeDuration += Time.deltaTime;
                var t = Mathf.InverseLerp(0f, maxChargeTime, chargeDuration);
                dmgMultiplier = Mathf.Lerp(minDmgMultiplier, maxDmgMultiplier, t);
            }
        }
    }

    private void Shoot()
    {
        shooter.transform.position = bow.transform.position;
        shooter.transform.forward = transform.forward;
        shooter.Shoot(chargeDuration > 0 ? chargedProjectilePrefab : projectilePrefab, dmgMultiplier);
    }
}
