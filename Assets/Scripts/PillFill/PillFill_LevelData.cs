using UnityEngine;

[CreateAssetMenu(fileName = "PillFill_LevelData", menuName = "Scriptable Objects/PillFill_LevelData")]
public class PillFill_LevelData : ScriptableObject
{
    [Header("Level Settings")]
    public int pillCount = 2;          // draggable items
    public int obstacleCount = 1;      // bandages / meds (NOT draggable)

    [Header("Pill Variants")]
    public GameObject[] pillPrefabs;

    [Header("Obstacle Variants")]
    public GameObject[] obstaclePrefabs;

    [Header("Drop Box Requirement")]
    public int requiredPillCount = 2;  // how many pills must be dropped

    [Header("Optional Visuals")]
    //public Sprite background;
    public string instructionText = "Collect the pills!";
}




