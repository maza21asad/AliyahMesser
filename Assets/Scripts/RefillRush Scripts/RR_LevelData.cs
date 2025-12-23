using UnityEngine;

[CreateAssetMenu(fileName = "RR_LevelData", menuName = "Scriptable Objects/RR_Level Data")]
public class RR_LevelData : ScriptableObject
{
    public GameObject[] itemPrefabs;
    public GameObject[] slotPrefabs;

    public int requiredCorrectPlacements;
    public string instructionText;
}
