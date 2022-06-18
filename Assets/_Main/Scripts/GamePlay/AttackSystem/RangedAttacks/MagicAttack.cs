using UnityEngine;

public class MagicAttack : RangedAttack
{
    public override void DoOnAimStart()
    {
        Debug.Log("testing magic start");
    }

    public override void DoOnAimEnd()
    {
        Debug.Log("testing magic end");
    }

    public override void DoOnAimCanceled()
    {
        Debug.Log("testing magic canceled");
    }
}
