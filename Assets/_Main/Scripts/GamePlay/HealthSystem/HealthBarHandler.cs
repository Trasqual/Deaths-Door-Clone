using System.Collections.Generic;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] GameObject healthBarGroup;
    [SerializeField] HealthBarSet healthBar;

    HealthManagerBase healthManager;

    List<HealthBarSet> barSets = new List<HealthBarSet>();

    private void Start()
    {
        healthManager = GetComponentInParent<HealthManagerBase>();
        healthManager.OnDamageTaken += UpdateHealthBar;
        healthManager.OnDeath += DisableHealthBar;
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < healthManager.MaxHealth; i++)
        {
            barSets.Add(Instantiate(healthBar, transform.GetChild(0)));
        }
    }

    private void UpdateHealthBar(int value)
    {
        for (int i = 0; i < barSets.Count; i++)
        {
            if (i < value)
            {
                barSets[i].ActivateBar();
            }
            else
            {
                barSets[i].CloseBar();
            }
        }
    }

    private void DisableHealthBar()
    {
        healthBarGroup.SetActive(false);
    }
}
