using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.GamePlay.UI
{
    public class VisualizableBarHandler : MonoBehaviour
    {
        [SerializeField] private GameObject barGroup;
        [SerializeField] private VisualizableBar bar;
        [SerializeField] private IVisualizable _source;

        private readonly List<VisualizableBar> barSets = new();

        public void Initialize(IVisualizable source)
        {
            _source = source;
            _source.OnValueChanged += UpdateHealthBar;
            _source.OnMaxValueChanged += SetupVisual;
            _source.OnClose += DisableHealthBar;

            SetupVisual();
        }

        private void SetupVisual()
        {
            for (int i = 0; i < barSets.Count; i++)
            {
                Destroy(barSets[i].gameObject);
            }
            barSets.Clear();
            for (int i = 0; i < _source.GetMaxValue(); i++)
            {
                barSets.Add(Instantiate(bar, transform.GetChild(0)));
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
            barGroup.SetActive(false);
        }

        private void OnDisable()
        {
            _source.OnValueChanged -= UpdateHealthBar;
            _source.OnClose -= DisableHealthBar;
        }
    }
}