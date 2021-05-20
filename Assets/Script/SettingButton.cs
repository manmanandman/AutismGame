using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    public GameObject audioControlUI;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAudioUI()
    {
        audioControlUI.SetActive(!audioControlUI.activeSelf);
    }
}
