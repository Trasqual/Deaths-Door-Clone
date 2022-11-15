using UnityEngine;

namespace _Main.Scripts.GamePlay.HealthSystem
{
    public class HealthBarSet : MonoBehaviour
    {
        [SerializeField] GameObject bar;

        public void CloseBar()
        {
            bar.SetActive(false);
        }

        public void ActivateBar()
        {
            bar.SetActive(true);
        }
    }
}