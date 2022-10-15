using DG.Tweening;
using UnityEngine;

public class ManifestAttackEventListener : MonoBehaviour
{
    [SerializeField] GameObject swordManifest;

    public void ActivateSword()
    {
        swordManifest.SetActive(true);
        swordManifest.GetComponent<Animator>().Play("SwordManifest");
    }

    public void DeactivateSword()
    {
        swordManifest.GetComponent<Animator>().Play("SwordManifestDeactivate");
        DOVirtual.DelayedCall(0.5f, () => swordManifest.SetActive(false));
    }
}
