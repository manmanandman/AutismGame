using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    private Animator animator;
    public float timeToShowHelpButton = 30f;
    public float timeToStopGame = 45f;
    public float counter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            counter = 0;
        else
        {
            counter += Time.deltaTime;
        }

        if (counter >= timeToStopGame)
        {
            // end game and back to menu
            ModuleManager.Instance.ReplayALLModule();
        }
        else if (counter >= timeToShowHelpButton)
        {
            // show button
            //animator.SetTrigger("On");
            ModuleManager.Instance.ButtonHelp();
        }
    }

    public void Clicked()
    {
        animator.SetTrigger("Off");
    }
    
}
