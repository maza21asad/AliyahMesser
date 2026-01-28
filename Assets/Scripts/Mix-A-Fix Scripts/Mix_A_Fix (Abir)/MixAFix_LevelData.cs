using UnityEngine;

// This adds the menu item: Right Click -> Create -> MixAFix -> Level Data
[CreateAssetMenu(fileName = "MixLevel_1", menuName = "MixAFix/Level Data")]
public class MixAFix_LevelData : ScriptableObject
{
    [Header("Scoop Requirements")]
    public int requiredPowder;
    public int requiredPinkCream;
    public int requiredYellowCream;
    public int requiredDrop;
}