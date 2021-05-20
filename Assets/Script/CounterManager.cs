using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterManager : MonoBehaviour
{
    private static CounterManager instance;
    public static CounterManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<CounterManager>();

            if (instance == null)
                Debug.Log("There is no Counter Manager");

            return instance;
        }
    }

    public GameObject Clock;
    public Text timeText;

    public bool usingClockDisplay = false;
    public float timeToHint = 30f;
    public float time = 0f;
    public float timeToStopGame = 45f;
    public float counterForRestart = 0f;
    public bool isEnable = false;

    void Start()
    {
        Clock = this.transform.GetChild(0).gameObject;
        timeText = Clock.transform.GetChild(0).GetComponent<Text>();
        SetDisplayCounter(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            counterForRestart = 0;
        if (isEnable)
        {
            time += Time.deltaTime;
            counterForRestart += Time.deltaTime;
        }
        if (counterForRestart >= timeToStopGame)
        {
            // end game and back to menu
            SetEnableCounter(false);
            counterForRestart = 0;
            ModuleManager.Instance.ReplayALLModule();
            DialogueManager.Instance.EndDialogue();
            AudioManager.Instance.voiceOverSource.Stop();
        }
        else if (time >= timeToHint)
        {
            // show button
            time = 0;
            ModuleManager.Instance.ButtonHelp();
        }
        DisplayTime(time);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (minutes == 100)
        {
            time = 0;
            minutes = 0;
            seconds = 0;
        }

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetCounter()
    {
        time = 0;
    }
    public void SetEnableCounter(bool enable)
    {
        isEnable = enable;
    }
    public void SetDisplayCounter(bool enable)
    {
        if(usingClockDisplay)
            Clock.gameObject.SetActive(enable);
    }


    public float GetTime()
    {
        return time;
    }

}
