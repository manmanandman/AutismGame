using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SweepingModule : Module
{
    public GameObject think_1;
    public GameObject think_2;
    public GameObject think_3;
    public GameObject hand_1;
    public GameObject hand_2;
    public GameObject hand_3;
    public GameObject broom;
    public GameObject dustPan;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject trashes;

    public int state;
    public override void ResetModule()
    {
        state = 0;

        think_1.SetActive(true);
        think_2.SetActive(false);
        think_3.SetActive(false);

        hand_1.SetActive(true);
        hand_2.SetActive(false);
        hand_3.SetActive(false);

        broom.SetActive(false);
        dustPan.SetActive(false);

        leftArrow.SetActive(false);
        rightArrow.SetActive(false);

        trashes.SetActive(true);



        LayerBackground[0].gameObject.SetActive(true);
        LayerInteractable[0].gameObject.SetActive(true);
    }
    
    public void ClickTrashes()
    {
        if (state == 0)
        {
            state = 1;
            think_1.SetActive(false);
            think_2.SetActive(true);

            hand_1.SetActive(false);
            hand_2.SetActive(true);

            broom.SetActive(true);
            dustPan.GetComponent<Button>().enabled = false;
            dustPan.SetActive(true);

            trashes.GetComponent<Button>().enabled = false;
        }
        else if (state == 1)
        {
            state = 2;
            rightArrow.SetActive(false);
            leftArrow.SetActive(true);
        }
        else if (state == 2)
        {
            state = 3;
            leftArrow.SetActive(false);
            hand_3.SetActive(true);
            think_3.SetActive(true);
            think_2.SetActive(false);
            dustPan.GetComponent<Button>().enabled = true;
        }
    }
}
