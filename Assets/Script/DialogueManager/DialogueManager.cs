using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;
    public GameObject dialogBox;
    private static DialogueManager instance;
    public bool useThaiFontAdjuster = true;
    public static DialogueManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<DialogueManager>();

            if (instance == null)
                Debug.Log("There is no Dialogue Manager");

            return instance;
        }
    }
    public Text dialogueText;
    private Queue<string> sentences;


    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue (string[] dialogues)
    {
        dialogBox.gameObject.SetActive(true);
        animator.SetBool("IsOpen", true);
        sentences.Clear();

        foreach(string dialogue in  dialogues)
        {
            sentences.Enqueue(dialogue);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if(useThaiFontAdjuster)
            sentence = ThaiFontAdjuster.Adjust(sentence);
        dialogueText.text = sentence;
    }

    public void EndDialogue ()
    {
        sentences.Clear();
        animator.SetBool("IsOpen", false);
    }

    public void ForceSetText (string text)
    {
        if (useThaiFontAdjuster)
            text = ThaiFontAdjuster.Adjust(text);
        dialogueText.text = text;
    }

}
