using _Main.Scripts.GamePlay.UI;
using System;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AmmunitionSystem
{
    public class AmmoCounter : MonoBehaviour, IVisualizable
    {
        [SerializeField] private int maxAmmo = 4;
        [SerializeField] private Vector3 visualizerPosition;

        public int CurrentAmmo { get; private set; }

        private void Awake()
        {
            CurrentAmmo = maxAmmo;
            var visualizer = Instantiate(visualizerPrefab, transform.TransformPoint(visualizerPosition), transform.rotation, transform);
            visualizer.Initialize(this);
        }

        public void GainAmmo(int count)
        {
            CurrentAmmo += count;
            CurrentAmmo = Mathf.Min(maxAmmo, CurrentAmmo);
            OnValueChanged?.Invoke(CurrentAmmo);
        }

        public void UseAmmo()
        {
            if (CurrentAmmo > 0)
                CurrentAmmo--;

            CurrentAmmo = Mathf.Max(0, CurrentAmmo);
            OnValueChanged?.Invoke(CurrentAmmo);
        }

        public void UpgradeAmmoCapacity(int amount)
        {
            maxAmmo += amount;
            OnMaxValueChanged?.Invoke();
        }

        private void OnDisable()
        {
            OnClose?.Invoke();
        }

        #region IVisualizable
        public Action<float> OnValueChanged { get; set; }
        public Action OnMaxValueChanged { get; set; }
        public Action OnClose { get; set; }
        [field: SerializeField] public VisualizableBarHandler visualizerPrefab { get; set; }


        public int GetMaxValue() => maxAmmo;
        #endregion
    }
}