using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{

    public static DialogController Instance { get; private set; }

    public GameObject dialogPanel;
    public GameObject hotBar;
    public TMP_Text dialogText, nameText;
    public Image portraitImage;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // make sure theres only one 
    }

    public void showDialogUI(bool show)
    {
        dialogPanel.SetActive(show);
        hotBar.SetActive(!show);

    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName;
        portraitImage.sprite = portrait;
    }

    public void SetDialogText(string text)
    {
        dialogText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer) Destroy(child.gameObject);
    }

    public GameObject CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onclick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onclick);
        return choiceButton;
    }
}
