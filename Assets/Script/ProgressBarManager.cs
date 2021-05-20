using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
    private static ProgressBarManager instance;
    public static ProgressBarManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ProgressBarManager>();

            if (instance == null)
                Debug.Log("There is no Progress Bar Manager");

            return instance;
        }
    }

    private GameObject progressBar;
    void Start()
    {
        progressBar = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnableProgressBar(bool enable)
    {
        progressBar.SetActive(enable);
    }
}
