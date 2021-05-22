using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    /** 
     * This class is a reference class to use for all module in future
     * All modules is controlled by "ModuleManager" Class 
     */

    public int Order; // auto assign
    public string moduleName; // manual assign
    public bool complete = false; // auto assign
    public bool hasCompleted = false; // auto assign

    // manual assign
    public List<Canvas> LayerBackground = new List<Canvas>(); 
    public List<Canvas> LayerInteractable = new List<Canvas>();
    public List<AudioClip> BGM = new List<AudioClip>();
    public List<AudioClip> VoiceOver = new List<AudioClip>();
    public List<AudioClip> SFX = new List<AudioClip>();
    public string[] sentences;

    public AudioClip correctSound;


    // auto assign
    [HideInInspector] public GameObject Hint; // not using anymore in latest update meeting
    [HideInInspector] public GameObject LayerResult; // auto assign ( can be null if not have "GetStar()" function  )
    [HideInInspector] public List<bool> checkVO = new List<bool>();

    public void OnEnable() // run every time that module start
    {
        Debug.Log("Playing Module : " + moduleName);

        foreach (var layer in LayerBackground)
        {
            layer.gameObject.SetActive(false);
        }
        foreach (var layer in LayerInteractable)
        {
            layer.gameObject.SetActive(false);
        }

        if (LayerBackground.Count != 0)
            LayerBackground[0].gameObject.SetActive(true);

        if (LayerInteractable.Count != 0)
            LayerInteractable[0].gameObject.SetActive(true);

        if (BGM.Count != 0)
        {
            AudioManager.Instance.PlayBGMAudio(BGM[0]);
        }

        checkVO.Clear();
        for (int i = 0; i < VoiceOver.Count; i++)
        {
            checkVO.Add(false);
        }
    }
    public void SetComplete() // complete module but dont show result ex. sunrise/wakeup01 module
    {
        complete = true;
        hasCompleted = true;
        Debug.Log("Complete Module : " + moduleName);
    }

    public void SetCompleteWithResult(int star) // complete module and show result 
    {
        hasCompleted = true;
        Debug.Log("Complete Module : " + moduleName + " with " + star + " star(s) in " + CounterManager.Instance.GetTime().ToString("#.##") + " second(s).") ;
        if (star > 0)
        {
            LayerResult.SetActive(true);
            LayerResult.GetComponent<GetStar>().SetNextButtonEnable();
            LayerResult.GetComponent<GetStar>().SetStar(star);
        }
        else
        {
            LayerResult.SetActive(true);
        }
    }

    public void TriggerDialogue ()  // open dialogue box and display first sentence from list "sentences"
    {
        DialogueManager.Instance.StartDialogue(sentences);
    }

    public void DisplayNextDialogue () // display next sentence  from list "sentences" and if no sentence left, close dialogue box 
    {
        DialogueManager.Instance.DisplayNextSentence();
    }
    public void PlayVO(int index) // play voice over from list "VoiceOver"
    {
        AudioManager.Instance.PlayVoiceOver(VoiceOver[index], checkVO[index]);
        checkVO[index] = true;
    }

    /* virtual function need to manual script differently up to  each module */

    public virtual void PlayingModule() { } // loop fuction every frame 

    public virtual void ResetModule() { } // reset any component in game to initial state. Also run every time that module start

    public virtual void GetHelp() { } // auto show hint while time reach limit

}
