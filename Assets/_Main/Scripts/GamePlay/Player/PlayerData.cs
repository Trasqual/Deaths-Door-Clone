using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [TitleGroup("Base")]
    [SerializeField] private string name = "";
    
    [TitleGroup("Locomotion")]
    [SerializeField] private float movementSpeed = 5F;
    [SerializeField] private float rotationSpeed = 5F;
    [SerializeField] private float dodgeSpeed = 5F;

    [TitleGroup("Others")]
    [SerializeField] private int health = 5;
}