using UnityEngine;

public class HealthBarSet : MonoBehaviour
{
    [SerializeField] private GameObject _bar;

    public void CloseBar()
    {
        _bar.SetActive(false);
    }

    public void ActivateBar()
    {
        _bar.SetActive(true);
    }
}
