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
            //GameObject prefab = data.pillPrefabs[Random.Range(0, data.pillPrefabs.Length)];
            //GameObject pill = Instantiate(prefab, spawnParent);
            //pill.GetComponent<PillFill_DragItem>().itemCategory = "Pill";

            //GameObject prefab = data.pillPrefabs[i];   // exact pill
            //Instantiate(prefab, spawnParent);

            GameObject prefab = data.pillPrefabs[i];
            GameObject pill = Instantiate(prefab, spawnParent);
            pill.GetComponent<PillFill_DragItem>().itemCategory = "Pill";

            //GameObject pill = Instantiate(data.pillPrefab, spawnParent);
            //pill.GetComponent<PillFill_DragItem>().itemCategory = "Pill";

        }

        // Spawn obstacles (NOT draggable)
        for (int i = 0; i < data.obstacleCount; i++)
        {
            //GameObject prefab = data.obstaclePrefabs[Random.Range(0, data.obstaclePrefabs.Length)];
            //Instantiate(prefab, obstacleParent);
            GameObject prefab = data.obstaclePrefabs[i];
            Instantiate(prefab, obstacleParent);
            //Instantiate(data.obstaclePrefab, obstacleParent);
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
