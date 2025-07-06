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
    // public GameObject dialogPanel;
    // public TMP_Text dialogText, nameText;
    // public Image portraitImage;

    private int dialogIndex;
    private bool isTyping, isDialogActive;

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
        isDialogActive = true;
        dialogIndex = 0;

        dialogUI.SetNPCInfo(dialogData.npcName, dialogData.npcPortrait);
        dialogUI.showDialogUI(true);

        PauseController.SetPause(true);

        DisplayCurrentLine();
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
            dialogUI.CreateChoiceButton(choice.choices[i], () => ChooseOption(nextIndex));
        }
    }

    void DisplayCurrentLine() {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    void ChooseOption(int nextIndex) {
        dialogIndex = nextIndex;
        dialogUI.ClearChoices();
        DisplayCurrentLine();
    }

    public void EndDialog()
    {
        StopAllCoroutines();
        isDialogActive = false;
        dialogUI.SetDialogText("");
        dialogUI.showDialogUI(false);
        PauseController.SetPause(false);

    }

}
