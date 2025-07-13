using UnityEngine;

public class RewardsController : MonoBehaviour
{
    public static RewardsController Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void GiveQuestReward(Quest quest)
    {
        if (quest?.rewards == null) {return;}
        foreach (var reward in quest.rewards) {
            switch (reward.type)
            {
                case RewardType.Item:
                    // give item
                    GiveItemReward(reward.rewardID, reward.amount);
                    break;
                case RewardType.Pennies:
                    // give penny

                    break;
                case RewardType.Experience:
                    // give exp

                    break;
                case RewardType.Custom:
                    // give something

                    break;
            }

        }
        
    }

    public void GiveItemReward(int itemID, int amount)
    {
        // Debug.Log($"Attempting to give item reward with ID: {itemID} and amount: {amount}");

        var itemPrefab = FindAnyObjectByType<ItemDictionary>()?.GetItemPrefab(itemID);
        if (itemPrefab == null)
        {
            //Debug.LogError($"Item prefab with ID {itemID} not found.");
            return;
        }


        for (int i = 0; i < amount; i++)
        {
            //Debug.Log($"Attempting to add item {i + 1} of {amount} to inventory.");
            if (!InventoryController.Instance.AddItem(itemPrefab))
            {
                GameObject dropitem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
                Debug.Log("inventory too full to get reward");
                // dropitem.GetComponent<BounceEffect>().StartBounce();
            }
            else
            {
                Debug.Log($"added item: {itemPrefab.name} to inventory.");
                //show popup
                itemPrefab.GetComponent<Item>().ShowPopup();
            }
        }
    }
}
