using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialog", menuName = "NPC Dialog")]
public class NPCDialog : ScriptableObject
{

    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogLines;
    public bool[] autoProgressLines;
    public bool[] endDialogLines;
    public float autoProgreeDelay = 1.5f;
    public float typingSpeed = 0.05f;
    // public AudioClip voiceSound;
    // public float voicePitch = 1f;

    public DialogChoice[] choices;

}

[System.Serializable]

public class DialogChoice
{
    public int dialogIndex; // line where choices apear
    public string[] choices; // player response options
    public int[] nextDialogIndexes; // where choice leads




}