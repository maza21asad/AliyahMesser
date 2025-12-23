using UnityEngine;

public class RR_ItemSpawner : MonoBehaviour
{
    public static RR_ItemSpawner Instance;

    [Header("Parents")]
    public Transform itemParent;   // draggable items
    public Transform slotParent;   // drop slots

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Spawn(RR_LevelData data)
    {
        ClearPrevious();

        // Spawn draggable items
        foreach (GameObject itemPrefab in data.itemPrefabs)
        {
            Instantiate(itemPrefab, itemParent);
        }

        // Spawn drop slots
        foreach (GameObject slotPrefab in data.slotPrefabs)
        {
            Instantiate(slotPrefab, slotParent);
        }
    }

    private void ClearPrevious()
    {
        foreach (Transform child in itemParent)
            Destroy(child.gameObject);

        foreach (Transform child in slotParent)
            Destroy(child.gameObject);
    }
}
