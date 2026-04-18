using UnityEngine;

[CreateAssetMenu(fileName = "PowerupSO", menuName = "ScriptableObjects/PowerupSO")]
public class PowerupSO : ScriptableObject
{
    public int healthBoost = 0;
    public int speedBoost = 0;
    public int grappleCount = 0;
}
