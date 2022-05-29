using UnityEngine;

[RequireComponent(typeof(PlayerController), 
    typeof(PlayerAnimationController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData data;

    public PlayerData Data => data;
}