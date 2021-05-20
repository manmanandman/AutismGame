using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WashDishModule : Module
{
    public Image kid1;
    public List<Sprite> boy;
    public List<Sprite> girl;

    //spite order
    //0 = holdish
    //1 = happy

    public GameObject lHand;
    public GameObject rHand;
    public List<Sprite> lHand_s;
    public List<Sprite> rHand_s;

    //spite order
    // 0 : L = ฟองน้ำ      R = ถือขวดน้ำยาล้างจาน
    // 1 : L = ฟองน้ำมีน้ำยา  R = ขวดน้ำยามีหยดน้ำ
    // 2 :                R = ถือจานเปื้อน
    // 3 :                R = ถือจานมีฟอง

    public Vector3 initPosRHand;
    public Vector3 initPosLHand;
    public bool holding;
    public bool liquidOvering;
    public bool passRubbingDish;

    private Vector3 deltaClick;
    private string item;

    public Animator RHandAnimator;
    public Animator LHandAnimator;
    
    public override void ResetModule()
    {
        if (ModuleManager.Instance.characterGender)
        {
            kid1.sprite = boy[0];
        }
        else
        {
            kid1.sprite = girl[0];
        }
        item = "empty";
        LayerBackground[0].gameObject.SetActive(true);
        LayerInteractable[0].gameObject.SetActive(true);
        LayerBackground[1].gameObject.SetActive(false);
        LayerInteractable[1].gameObject.SetActive(false);
        rHand.GetComponent<Image>().sprite = rHand_s[0];
        lHand.GetComponent<Image>().sprite = lHand_s[0];
        lHand.transform.GetChild(0).gameObject.SetActive(false); //collider left hand
        rHand.transform.GetChild(0).gameObject.SetActive(true); //collider right hand
        initPosRHand = rHand.transform.localPosition;
        initPosLHand = lHand.transform.localPosition;
        holding = false;
        liquidOvering = false;
        passRubbingDish = false;

    }
    
    public void ClickSink()
    {
        LayerBackground[0].gameObject.SetActive(false);
        LayerInteractable[0].gameObject.SetActive(false);
        LayerBackground[1].gameObject.SetActive(true);
        LayerInteractable[1].gameObject.SetActive(true);
    }

    public void SlideHandToHoldDish()
    {
        //animation เลื่อนมือขวาไปทางขวาจนหายไปจากจอ (16:9) แล้วเปลี่ยน sprite เป็นมือถือจาน
        rHand.transform.GetChild(0).gameObject.SetActive(false); //collider right hand
        rHand.GetComponent<PolygonCollider2D>().enabled = false;
        StartCoroutine(ChangeRhandSprite(rHand_s[2]));
        lHand.GetComponent<Image>().sprite = lHand_s[1];
        lHand.transform.GetChild(0).gameObject.SetActive(true); //collider left hand
        lHand.GetComponent<PolygonCollider2D>().enabled = false;
        lHand.transform.SetAsLastSibling();
    }

    IEnumerator ChangeRhandSprite(Sprite sprite)
    {
        AnimationClip[] clips = RHandAnimator.runtimeAnimatorController.animationClips;
        RHandAnimator.SetBool("IsIn", false);
        yield return new WaitForSeconds(clips[1].length);
        rHand.GetComponent<Image>().sprite = sprite;
        rHand.transform.localPosition = initPosRHand;
        RHandAnimator.SetBool("IsIn", true);
    }

    public void PassRubingToWashing()
    {
        //ย้ายมือซ้ายออกนอกจอ ย้ายมือขวามาตรงกลาง
        LHandAnimator.SetBool("IsIn", false);
        RHandAnimator.SetBool("IsMiddle", true);
    }

    public override void PlayingModule()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (rHand.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition))
            {
                deltaClick = Input.mousePosition - rHand.transform.position;
                holding = true;
                item = "rHand";
            }
            else if (lHand.transform.GetChild(0).GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition) || lHand.transform.GetChild(1).GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition))
            {
                deltaClick = Input.mousePosition - lHand.transform.position;
                holding = true;
                item = "lHand_child_0";
            }
        }
        else if (Input.GetMouseButton(0) && holding)
        {
            if (item.Contains("rHand"))
            {
                rHand.transform.position = Input.mousePosition - deltaClick;
            }
            else if (item.Contains("lHand"))
            {
                lHand.transform.position = Input.mousePosition - deltaClick;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (liquidOvering)
            {
                liquidOvering = false;
                SlideHandToHoldDish();
            }

            if (passRubbingDish)
            {
                passRubbingDish = false;
                PassRubingToWashing();
            }

            if (item.Contains("rHand") && rHand.GetComponent<Image>().sprite != rHand_s[2])
            {
                rHand.transform.localPosition = initPosRHand;
            }
            else if (item.Contains("lHand"))
            {
                lHand.transform.localPosition = initPosLHand;
            }

            if (!item.Equals("empty"))
            {
                item = "empty";
            }

            holding = false;
        }
        else
        {
            
        }

        if (passRubbingDish) //change sprite await mouse up
        {
            rHand.GetComponent<Image>().sprite = rHand_s[3];
        }
    }
}
