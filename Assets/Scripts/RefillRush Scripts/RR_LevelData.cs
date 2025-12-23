using UnityEngine;

[CreateAssetMenu(fileName = "RR_LevelData", menuName = "RefillRush/Level Data")]
public class RR_LevelData : ScriptableObject
{
    public GameObject[] itemPrefabs;
    public GameObject[] slotPrefabs;

    public int requiredCorrectPlacements;
    public string instructionText;
}
