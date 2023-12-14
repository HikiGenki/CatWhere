using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings", order = 0)]
public class GameSettings : ScriptableObject
{
    public float initialWait = 0f;

    [Header("Timer")]
    public float timerSpeed = 0.2f;

    public float timerSpeedIncrease = 0.2f;
}