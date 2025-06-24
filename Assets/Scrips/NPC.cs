using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// public class NPC : MonoBehaviour, IInteractable
public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialog dialogData;
    public GameObject dialogPanel;
    public TMP_Text dialogText, nameText;
    public UnityEngine.UI.Image portraitImage;

    private int dialogIndex;
    private bool isTyping, isDialogActive;


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
            dialogText.SetText(dialogData.dialogLines[dialogIndex]);
            isTyping = false;

        }
        else if (++dialogIndex < dialogData.dialogLines.Length)
        {
            StartCoroutine(TypeLine());
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
        nameText.SetText(dialogData.npcName);
        portraitImage.sprite = dialogData.npcPortrait;
        dialogPanel.SetActive(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogText.SetText("");

        foreach (char letter in dialogData.dialogLines[dialogIndex])
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(dialogData.typingSpeed);
        }

        isTyping = false;

        if (dialogData.autoProgressLines.Length > dialogIndex && dialogData.autoProgressLines[dialogIndex])
        {
            yield return new WaitForSeconds(dialogData.autoProgreeDelay);

            NextLine();
        }
    }

    public void EndDialog()
    {
        StopAllCoroutines();
        isDialogActive = false;
        dialogText.SetText("");
        dialogPanel.SetActive(false);
        PauseController.SetPause(false);

    }

}
