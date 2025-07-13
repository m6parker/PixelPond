using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Cinemachine;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    public CinemachineCamera virtualCamera;
    // private Chest[] chests;
    // public GameObject controlsPanel;



    [System.Obsolete]
    void Start()
    {
        InitializeComponents();

        LoadGame(); // need load game option in menu or start new game

    }

    [System.Obsolete]
    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();

        virtualCamera = FindObjectOfType<CinemachineCamera>();
        // chests = FindObjectsByType<Chest>();
    }

    // [System.Obsolete]
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),
            questProgressData = QuestContoller.Instance.activateQuests
            // chestSaveData = GetChestsState();
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void SaveAndQuit()
    {
        SaveGame();
        Debug.Log("saving game...");
        Debug.Log("Quit Game");
        Application.Quit();
    }

    // public void BackButton()
    // {
    //     controlsPanel.SetActive(false);
    // }

    // private List<ChestSaveData> GetChestsState()
    // {
    //     List<ChestSaveData> chestStates = new List<ChestSaveData>();
    //     foreach (Chest chest in chests)
    //     {
    //         ChestSaveData chestSaveData = new ChestSaveData
    //         {
    //             chestID = chest.ChestID,
    //             isOpened = chest.isOpened

    //         };
    //         chestStates.Add(chestSaveData);
    //     }
    //     return chestStates;
    // }

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

            QuestContoller.Instance.LoadQuestProgess(saveData.questProgressData);
            // LoadChestsState(saveData.chestSaveData);

            // Debug.Log("mapboundary " + FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D);

        }
        else
        {

            inventoryController.SetInventoryItems(new List<InventorySaveData>());
            hotbarController.SetHotbarItems(new List<InventorySaveData>());

            SaveGame();
        }
        virtualCamera.PreviousStateIsValid = false;

    }

    // private void LoadChestsState()
    // {
    //     foreach (Chest chest in chests)
    //     {
    //         ChestSaveData chestSaveData = chestSates.FirstOrDefault(c => c.chestID == chest.ChestID)
    //         if(chestSaveData != null){
    //          chest.SetOpened(chestSaveData.isOpened);    
    //          }
    //     }
    // }
}
