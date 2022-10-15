using UnityEngine;

public class ManifestAttackEventListener : MonoBehaviour
{
    [SerializeField] GameObject swordManifest;

    public void ActivateSword()
    {
        swordManifest.SetActive(true);
    }

    public void DeactivateSword()
    {
        swordManifest.SetActive(false);
    }
}
