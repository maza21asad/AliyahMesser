using UnityEngine;

public class PillFill_ItemSpawner : MonoBehaviour
{
    public static PillFill_ItemSpawner Instance;

    public Transform spawnParent;   // assign Canvas panel in Inspector
    public Transform obstacleParent;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnLevel(PillFill_LevelData data)
    {
        ClearPrevious();

        // Spawn pills (draggable)
        for (int i = 0; i < data.pillCount; i++)
        {
            GameObject pill = Instantiate(data.pillPrefab, spawnParent);
            pill.GetComponent<PillFill_DragItem>().itemCategory = "Pill";
        }

        // Spawn obstacles (NOT draggable)
        for (int i = 0; i < data.obstacleCount; i++)
        {
            Instantiate(data.obstaclePrefab, obstacleParent);
        }
    }

    private void ClearPrevious()
    {
        foreach (Transform child in spawnParent)
            Destroy(child.gameObject);

        foreach (Transform child in obstacleParent)
            Destroy(child.gameObject);
    }
}
