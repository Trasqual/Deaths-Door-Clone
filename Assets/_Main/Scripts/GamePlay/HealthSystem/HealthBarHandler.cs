using System.Collections.Generic;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] private GameObject _healthBarGroup;
    [SerializeField] private HealthBarSet _healthBar;

    private HealthComponentBase _healthManager;

    private List<HealthBarSet> _barSets = new List<HealthBarSet>();

    private void Start()
    {
        _healthManager = GetComponentInParent<HealthComponentBase>();
        _healthManager.OnDamageTaken += UpdateHealthBar;
        _healthManager.OnDeath += DisableHealthBar;
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < _healthManager.MaxHealth; i++)
        {
            _barSets.Add(Instantiate(_healthBar, transform.GetChild(0)));
        }
    }

    private void UpdateHealthBar(int value)
    {
        for (int i = 0; i < _barSets.Count; i++)
        {
            if (i < value)
            {
                _barSets[i].ActivateBar();
            }
            else
            {
                _barSets[i].CloseBar();
            }
        }
    }

    private void DisableHealthBar()
    {
        _healthBarGroup.SetActive(false);
    }
}
