using System.Collections;
// using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// public class NPC : MonoBehaviour, IInteractable
public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialog dialogData;
    private DialogController dialogUI;
    private int dialogIndex;
    private bool isTyping, isDialogActive;

    private enum QuestState { NotStarted, InProgress, Completed }
    private QuestState questState = QuestState.NotStarted;

    private void Start()
    {
        dialogUI = DialogController.Instance;
    }

    public bool CanInteract()
    {
        return !isDialogActive;
    }

    public void Interact()
    {
        if (dialogData == null || (PauseController.isGamePaused && !isDialogActive))
        {
            return;
        }

        if (isDialogActive)
        {
            NextLine();
        }
        else
        {
            StartDialog();
        }
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogUI.SetDialogText(dialogData.dialogLines[dialogIndex]);
            isTyping = false;

        }

        //clear choices, check enddialog lines, check if choices and display
        dialogUI.ClearChoices();
        if (dialogData.endDialogLines.Length > dialogIndex && dialogData.endDialogLines[dialogIndex])
        {
            EndDialog();
            return;
        }

        foreach (DialogChoice dialogChoice in dialogData.choices)
        {
            if (dialogChoice.dialogIndex == dialogIndex)
            {
                //display choices
                DisplayChoices(dialogChoice);
                return;
            }
        }


        if (++dialogIndex < dialogData.dialogLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialog();
        }
    }

    void StartDialog()
    {
        // sync with quest data
        SyncQuestState();

        //set dialog based on queststaet
        if (questState == QuestState.NotStarted)
        {
            dialogIndex = 0;
        }
        else if (questState == QuestState.InProgress)
        {
            dialogIndex = dialogData.questInProgessIndex;

        }
        else if (questState == QuestState.Completed)
        {
            dialogIndex = dialogData.questCompletedIndex;

        }

        isDialogActive = true;

        dialogUI.SetNPCInfo(dialogData.npcName, dialogData.npcPortrait);
        dialogUI.showDialogUI(true);

        PauseController.SetPause(true);

        DisplayCurrentLine();
    }

    private void SyncQuestState()
    {
        if (dialogData.quest == null) return;
        string questID = dialogData.quest.questID;

        if (QuestContoller.Instance.IsQuestCompleted(questID) || QuestContoller.Instance.IsQuestHandedIn(questID))
        {
            questState = QuestState.Completed;
        }
        else if (QuestContoller.Instance.IsQuestActive(questID))
        {
            questState = QuestState.InProgress;
        }
        else
        {
            questState = QuestState.NotStarted;
        }

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogUI.SetDialogText("");

        foreach (char letter in dialogData.dialogLines[dialogIndex])
        {
            dialogUI.SetDialogText(dialogUI.dialogText.text += letter);
            yield return new WaitForSeconds(dialogData.typingSpeed);
        }

        isTyping = false;

        if (dialogData.autoProgressLines.Length > dialogIndex && dialogData.autoProgressLines[dialogIndex])
        {
            yield return new WaitForSeconds(dialogData.autoProgreeDelay);

            NextLine();
        }
    }

    void DisplayChoices(DialogChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogIndexes[i];
            bool givesQuest = choice.givesQuest[i];
            dialogUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex, givesQuest));
        }
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    void ChooseOption(int nextIndex, bool givesQuest)
    {
        if (givesQuest)
        {
            QuestContoller.Instance.AcceptQuest(dialogData.quest);
            questState = QuestState.InProgress;
        }
        dialogIndex = nextIndex;
        dialogUI.ClearChoices();
        DisplayCurrentLine();
    }

    public void EndDialog()
    {

        if (questState == QuestState.Completed && !QuestContoller.Instance.IsQuestHandedIn(dialogData.quest.questID))
        {
            // hand in completion
            HandleQuestCompletion(dialogData.quest);
        }

        StopAllCoroutines();
        isDialogActive = false;
        dialogUI.SetDialogText("");
        dialogUI.showDialogUI(false);
        PauseController.SetPause(false);

    }

    void HandleQuestCompletion(Quest quest)
    {
        RewardsController.Instance.GiveQuestReward(quest);
        QuestContoller.Instance.HandInQuest(quest.questID);
    }

}
