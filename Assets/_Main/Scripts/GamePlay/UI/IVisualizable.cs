using System;
using UnityEngine;

namespace _Main.Scripts.GamePlay.UI
{
    public interface IVisualizable
    {
        public Action<float> OnValueChanged { get; set; }
        public Action OnMaxValueChanged { get; set; }
        public Action OnClose { get; set; }

        public int GetMaxValue();
    }
}