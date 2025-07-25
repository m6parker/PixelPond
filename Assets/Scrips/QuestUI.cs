using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;
    public Quest testQuest;
    public int testQuestAmount;
    private List<QuestProgess> testQuests = new();

    void Start()
    {
        for (int i = 0; i < testQuestAmount; i++)
        {
            testQuests.Add(new QuestProgess(testQuest));
        }

        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        // destroy exsisting entries
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        //build new entries
        foreach (var quest in QuestContoller.Instance.activateQuests) // TestQuest for testing 
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Transform objectiveList = entry.transform.Find("ObjectiveList");

            questNameText.text = quest.quest.name;
            foreach (var objective in quest.objectives)
            {
                GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList);
                TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
                objText.text = $"{objective.description}\n( {objective.currentAmount} / {objective.requiredAmount} )";
            }
        }
    }
}
