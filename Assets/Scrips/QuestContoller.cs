using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class QuestContoller : MonoBehaviour
{
    public static QuestContoller Instance { get; private set; }

    public List<QuestProgess> activateQuests = new();
    private QuestUI questUI;
    public List<string> handInQuestIDs = new();


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        questUI = FindAnyObjectByType<QuestUI>();
        InventoryController.Instance.OnInventoryChanged += checkInventoryForQuests;
    }

    public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest.questID)) return;
        activateQuests.Add(new QuestProgess(quest));
        checkInventoryForQuests();
        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questUI) => activateQuests.Exists(q => q.QuestID == questUI);

    public void checkInventoryForQuests()
    {
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemsCount();
        foreach (QuestProgess quest in activateQuests)
        {
            foreach (QuestObjective questObjective in quest.objectives)
            {
                if (questObjective.type != ObjectiveType.CollectItem) continue;
                if (!int.TryParse(questObjective.objectiveID, out int itemID)) continue;

                int newAmount = itemCounts.TryGetValue(itemID, out int count) ? Mathf.Min(count, questObjective.requiredAmount) : 0;
                Debug.Log("Current Amount: " + questObjective.currentAmount);
                
                if (questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                }
                Debug.Log("newAmount: " + newAmount);

            }
        }
        Debug.Log("checkInventoryForQuests()");
        questUI.UpdateQuestUI();
    }

    public bool IsQuestCompleted(string questID) {
        QuestProgess quest = activateQuests.Find(q => q.QuestID == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.IsCompleted);
    }

    public void HandInQuest(string questID)
    {
        // remove items
        if (!RemoveRequiredItemsFromInventory(questID))
        {
            return; // missing items
        }

        // remove quest log
        QuestProgess quest = activateQuests.Find(q => q.QuestID == questID);
        if (quest != null)
        {
            handInQuestIDs.Add(questID);
            activateQuests.Remove(quest);
            questUI.UpdateQuestUI();
        }
    }

    public bool IsQuestHandedIn(string questID)
    {
        return handInQuestIDs.Contains(questID);
    }

    public bool RemoveRequiredItemsFromInventory(string questID)
    {
        QuestProgess quest = activateQuests.Find(q => q.QuestID == questID);
        if (quest != null) return false;

        Dictionary<int, int> requiredItems = new();

        foreach (QuestObjective objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.CollectItem && int.TryParse(objective.objectiveID, out int itemID))
            {
                requiredItems[itemID] = objective.requiredAmount;
            }
        }

        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemsCount();
        foreach (var item in requiredItems)
        {
            if (itemCounts.GetValueOrDefault(item.Key) < item.Value)
            {
                // not enough items to complete quest
                return false;
            }
        }

        foreach (var itemRequirement in requiredItems)
        {
            // remove from inventory
            InventoryController.Instance.RemoveItemsFromInventory(itemRequirement.Key, itemRequirement.Value);

        }
        return true;
    }

    public void LoadQuestProgess(List<QuestProgess> savedQuests)
    {
        activateQuests = savedQuests ?? new();
        checkInventoryForQuests();
        questUI.UpdateQuestUI();
    }
}
