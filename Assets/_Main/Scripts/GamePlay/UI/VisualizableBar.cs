using UnityEngine;

namespace _Main.Scripts.GamePlay.UI
{
    public class VisualizableBar : MonoBehaviour
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