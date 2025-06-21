using System.IO;
using Unity.Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    public CinemachineCamera virtualCamera;
    

    [System.Obsolete]
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();
        virtualCamera = FindObjectOfType<CinemachineCamera>();
        // if (virtualCamera != null) { virtualCamera.PreviousStateIsValid = false; }
        LoadGame();

    }

    // [System.Obsolete]
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    // [System.Obsolete]
    public void LoadGame()
    {
        Debug.Log("loading game");


        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
            FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            MapController_manual.Instance?.HighlightArea(saveData.mapBoundary);

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);

            Debug.Log("mapboundary " + FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D);

        }
        else
        {
            SaveGame();
        }
        virtualCamera.PreviousStateIsValid = false;

    }
}
