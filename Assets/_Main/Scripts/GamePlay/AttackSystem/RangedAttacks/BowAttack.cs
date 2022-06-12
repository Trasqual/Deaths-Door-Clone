using UnityEngine;

public class BowAttack : MonoBehaviour
{
    [Header("Animations//TODO")]
    [SerializeField] Animation bowAnim;
    [SerializeField] Animation bowRecoilAnim;

    [Header("Visuals")]
    [SerializeField] GameObject bow;
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Projectile chargedProjectilePrefab;

    [Header("Shooters")]
    [SerializeField] Shooter shooter;

    [Header("Attack Params")]
    [SerializeField] float initialChargeDelay = 1f;
    [SerializeField] float maxChargeTime = 3f;
    [SerializeField] float minDmgMultiplier = 1f;
    [SerializeField] float maxDmgMultiplier = 2f;
    float chargeDelayDuration;
    float chargeDuration;
    float dmgMultiplier;


    AimAction aimAction;
    AnimationBase anim;

    private void Start()
    {
        aimAction = GetComponentInParent<AimAction>();
        anim = GetComponentInParent<AnimationBase>();

        if (aimAction)
        {
            aimAction.OnActionStarted += PlayBowAnim;
            aimAction.OnActionEnded += PlayRecoilAnim;
            aimAction.OnActionPerformed += ChargeAttack;
        }
    }

    private void PlayBowAnim()
    {
        anim.PlayAimAnim(true);
        bow.SetActive(true);
    }

    private void PlayRecoilAnim()
    {
        anim.PlayAimAnim(false);
        Shoot();
        chargeDelayDuration = 0f;
        chargeDuration = 0f;
        dmgMultiplier = 1f;
        bow.SetActive(false);
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
