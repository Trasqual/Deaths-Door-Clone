using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] GameObject healthBarGroup;
    [SerializeField] Image healthBar;

    HealthManagerBase healthManager;

    private void Start()
    {
        healthManager = GetComponentInParent<HealthManagerBase>();
        healthManager.OnDamageTaken += UpdateHealthBar;
        healthManager.OnDeath += DisableHealthBar;
    }

    private void UpdateHealthBar(float value)
    {
        healthBar.fillAmount = value;
    }

    private void DisableHealthBar()
    {
        healthBarGroup.SetActive(false);
    }
}
