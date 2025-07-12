using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictionary;

    private void Awake()
    {
        itemDictionary = new Dictionary<int, GameObject>();
        //auto-inc
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1;
            }
        }

        foreach (Item item in itemPrefabs)
        {
            itemDictionary[item.ID] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {

        //     itemDictionary.TryGetValue(itemID, out GameObject prefab);
        //     Debug.Log($"Getting item with ID: {itemID} Prefab: {prefab} name: {prefab.name}");

        //     if (prefab == null)
        //     {
        //         Debug.LogWarning($"item with ID {itemID} not found in dictionary");
        //     }
        //     return prefab;

        // Check if the dictionary is initialized
        if (itemDictionary == null)
        {
            Debug.LogError("Item dictionary is not initialized.");
            return null;
        }

        // Try to get the prefab from the dictionary
        bool success = itemDictionary.TryGetValue(itemID, out GameObject prefab);

        // Log the result of the TryGetValue operation
        if (success)
        {
            Debug.Log($"Found item with ID: {itemID}. Prefab name: {prefab.name}");
        }
        else
        {
            Debug.LogWarning($"Item with ID {itemID} not found in dictionary.");
            return null;
        }

        // Check if the retrieved prefab is null
        if (prefab == null)
        {
            Debug.LogError($"Prefab for item with ID {itemID} is null.");
            return null;
        }

        return prefab;


    }
    
    public void LogItemDictionaryContents()
    {
        Debug.Log("Contents of itemDictionary:");
        foreach (var entry in itemDictionary)
        {
            Debug.Log($"Item ID: {entry.Key}, Prefab Name: {entry.Value.name}");
        }
    }

}
