using UnityEngine;

public class SwordAttack : MeleeAttackBase
{
    [SerializeField] GameObject swordVisual;

    protected override void DoOnActionStart()
    {
        base.DoOnActionStart();
        swordVisual.SetActive(true);
    }

    protected override void EndAttack()
    {
        base.EndAttack();
        swordVisual.SetActive(false);
    }
}
