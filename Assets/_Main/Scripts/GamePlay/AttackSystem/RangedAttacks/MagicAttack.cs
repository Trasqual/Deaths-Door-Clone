using UnityEngine;

public class MagicAttack : AttackBase
{
    public override void DoOnActionStart()
    {
        Debug.Log("testing magic start");
    }

    public override void DoOnActionEnd()
    {
        Debug.Log("testing magic end");
    }

    public override void DoOnActionCanceled()
    {
        Debug.Log("testing magic canceled");
    }
}
