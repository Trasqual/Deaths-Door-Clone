using UnityEngine;

public class SwordManifester : MonoBehaviour
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
