using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class QuestContoller : MonoBehaviour
{
    public static QuestContoller Instance { get; private set; }

    public List<QuestProgess> activateQuests = new();
    private QuestUI questUI;


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

    public void LoadQuestProgess(List<QuestProgess> savedQuests)
    {
        activateQuests = savedQuests ?? new();
        checkInventoryForQuests();
        questUI.UpdateQuestUI();
    }
}
