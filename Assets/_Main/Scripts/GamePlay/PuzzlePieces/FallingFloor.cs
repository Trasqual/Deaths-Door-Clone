using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Puzzles
{
    public class FallingFloor : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out CharacterController player))
            {
                Debug.Log("player detected");
            }
        }
    }
}

