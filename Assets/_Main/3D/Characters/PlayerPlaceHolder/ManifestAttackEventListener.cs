using DG.Tweening;
using UnityEngine;

public class ManifestAttackEventListener : MonoBehaviour
{
    [SerializeField] GameObject swordManifest;
    [SerializeField] GameObject earthShatterParticle;

    public void ActivateSword()
    {
        swordManifest.SetActive(true);
        swordManifest.GetComponent<Animator>().Play("SwordManifest");
    }

    public void DeactivateSword()
    {
        swordManifest.GetComponent<Animator>().Play("SwordManifestDeactivate");
        DOVirtual.DelayedCall(0.5f, () => swordManifest.SetActive(false));
        Instantiate(earthShatterParticle, transform.position + transform.forward, transform.rotation);
    }
}
