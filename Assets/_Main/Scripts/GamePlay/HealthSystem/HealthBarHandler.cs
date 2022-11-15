using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.GamePlay.HealthSystem
{
    public class HealthBarHandler : MonoBehaviour
    {
        [SerializeField] GameObject healthBarGroup;
        [SerializeField] HealthBarSet healthBar;

        HealthComponentBase healthManager;

        List<HealthBarSet> barSets = new List<HealthBarSet>();

        private void Start()
        {
            healthManager = GetComponentInParent<HealthComponentBase>();
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
}