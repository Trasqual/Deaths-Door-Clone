using UnityEngine;

namespace _Main.Scripts.GamePlay.AmmunitionSystem
{
    public class AmmoCounter : MonoBehaviour
    {
        [SerializeField] private int maxAmmo = 4;

        public int CurrentAmmo { get; private set; }

        private void Awake()
        {
            CurrentAmmo = maxAmmo;
        }

        public void GainAmmo(int count)
        {
            CurrentAmmo += count;
            CurrentAmmo = Mathf.Min(maxAmmo, CurrentAmmo);
        }

        public void UseAmmo()
        {
            if (CurrentAmmo > 0)
                CurrentAmmo--;

            CurrentAmmo = Mathf.Max(0, CurrentAmmo);
        }

        public void UpgradeAmmoCapacity(int amount)
        {
            maxAmmo += amount;
        }
    }
}