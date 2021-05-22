using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModuleManager : MonoBehaviour
{
    /**
     * Use this class to control all module that be child of this game object
     */

    [HideInInspector]  public List<Module> Modules = new List<Module>(); // auto assign

    public GameObject LayerResult; // manual assign
    public Canvas LayerFade; // manual assign

    [HideInInspector]  public bool characterGender = true; //true = boy, false = girl

    private static ModuleManager instance;
    public static ModuleManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ModuleManager>();

            if (instance == null)
                Debug.Log("There is no Module Manager");

            return instance;
        }
    }

    private Module currentModule;

    void Start()
    {
        // add module child to list
        var i = 0;
        foreach (Transform child in transform) 
        {
            Modules.Add(child.gameObject.GetComponent<Module>());
            child.GetComponent<Module>().Order = i;
            i++;
        }

        // set all module inactive
        foreach (var module in Modules)
        {
            module.gameObject.SetActive(false);
        }

        LayerFade.gameObject.SetActive(true);
        SetNextModule(0);
    }

    void Update()
    {
        // update function "PlayingModule" in each active module
        if (currentModule)
        {
            currentModule.PlayingModule();
        }
    }


    public void SetNextModule(int currentOrder)
    {
        LayerResult.SetActive(false);
        LayerResult.GetComponent<GetStar>().ResetNextButtonEnable();

        currentModule = GetModuleByOrder(currentOrder);

        if(currentModule)
        {
            currentModule.gameObject.SetActive(false);
            currentModule.LayerResult = LayerResult;
            currentModule.ResetModule();
            currentModule.gameObject.SetActive(true);


            //SetActiveListCanvas(currentModule.LayerBackground, true);
            //SetActiveListCanvas(currentModule.LayerInteractable, true);
        }

        if (!currentModule)
        {
            CompletedAllModule();
            return;
        }

    }

    public Module GetModuleByOrder(int Order)
    {
        //for (int i = 0; i < Modules.Count; i++)
        //{
        //    if (Modules[i].Order == Order)
        //        return Modules[i];
        //}
        foreach (var module in Modules)
        {
            if (module.Order == Order)
                return module;
        }

        return null;
    }

    public void CompletedAllModule()
    {
        Debug.Log("You have completed all Module!");
    }

    public void CompleteModule()
    {
        CounterManager.Instance.SetDisplayCounter(false);
        CounterManager.Instance.ResetCounter(); 

        currentModule.gameObject.SetActive(false);

        SetActiveListCanvas(currentModule.LayerBackground, false);
        SetActiveListCanvas(currentModule.LayerInteractable, false);

        SetNextModule(currentModule.Order + 1);
    }

    public void ReplayModule()
    {
        Debug.Log("Replay Module");
        CounterManager.Instance.SetDisplayCounter(false);
        CounterManager.Instance.ResetCounter();
        SetNextModule(currentModule.Order);
    }

    public void SetActiveCanvas(Canvas canvas,bool isActive)
    {
        if(canvas)
            canvas.gameObject.SetActive(isActive);
    }

    public void SetActiveListCanvas(List<Canvas> canvas, bool isActive)
    {
        for (int i = 0; i < canvas.Count; i++)
        {
            canvas[i].gameObject.SetActive(isActive);
        }
    }

    
    //****************** use when click play next button ******************//
    public void ButtonFadeChangeModule()
    {
        StartCoroutine(FadeChangeModule());
    }
    public IEnumerator FadeChangeModule()
    {
        LayerFade.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1);
        CompleteModule();
        LayerFade.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1);
    }

    //****************** use when click replay button ******************//
    public void ButtonFadeReplayModule()
    {
        StartCoroutine(FadeReplayModule());
    }
    public IEnumerator FadeReplayModule()
    {
        if(currentModule.VoiceOver.Count > 0)
        {
            currentModule.PlayVO(currentModule.VoiceOver.Count-1);
            yield return new WaitForSeconds(currentModule.VoiceOver[currentModule.VoiceOver.Count-1].length);
        }
        LayerFade.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1);
        ReplayModule();
        LayerFade.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        yield return new WaitForSeconds(1);
    }


    public void ReplayALLModule()
    {
        Debug.Log("Restart All Module");
        currentModule.gameObject.SetActive(false);
        SetActiveListCanvas(currentModule.LayerBackground, false);
        SetActiveListCanvas(currentModule.LayerInteractable, false);
        SetNextModule(0);
        //SceneManager.LoadScene(0);
    }

    public void ButtonHelp()
    {
        currentModule.GetHelp();
        //HintButton.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
