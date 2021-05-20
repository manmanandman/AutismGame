using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoldBlanketModule : Module
{
    private bool holding = false; //ตรวจสอบว่าคลิกค้างอยู่ (โดยเริ่มคลิกในพื้นที่ที่กำหนดไว้)
    private bool firstVO = false;

    private GameObject leftCollider; //พื้นที่คลิกพับผ้าด้านซ้าย
    private GameObject rightCollider; //พื้นที่ปล่อยคลิกพับผ้าด้านขวา
    private GameObject upCollider; //พื้นที่คลิกพับผ้า(ครั้งที่ 2)ด้านบน
    private GameObject downCollider; //พื้นที่ปล่อยคลิกพับผ้า(ครั้งที่ 2)ด้านล่าง
    private GameObject foldGame0; // พับผ้า ทบที่ 1
    private GameObject foldGame1; // พับผ้า ทบที่ 2
    private GameObject foldGame2; // พับผ้าเสร็จแล้ว
    private GameObject hand; // รูปมือที่วิ่งตามการคลิก

    public Sprite boy;
    public Sprite girl;
    public override void PlayingModule()
    {
        if (foldGame0.activeSelf) 
        {
            FoldingGame0();
        }
        else if (foldGame1.activeSelf)
        {
            FoldingGame1();
        }

        if (complete)
        {
            SetCompleteWithResult(3); //input star here
            //ModuleManager.Instance.LayerResult.SetActive(true);
            complete = false;
        }
        if(!firstVO)
        {
            firstVO = true;
            StartCoroutine(PlayFirstVO());
        }
    }

    public void ShowGame()
    {
        this.transform.GetChild(1).GetChild(0).gameObject.SetActive(false); // ปุ่ม ผ้าห่ม หน้าแรก (INTERACT)
        this.transform.GetChild(1).GetChild(1).gameObject.SetActive(false); // มือชี้ผ้าห่ม หน้าแรก (INTERACT)
        foldGame0.gameObject.SetActive(true);
        hand.gameObject.SetActive(false);
        DisplayNextDialogue();
    }

    private void FoldingGame0()
    {
        if (!hasCompleted)
            AudioManager.Instance.PlayVoiceOver(VoiceOver[2], checkVO[2]);
        checkVO[2] = true;
        if (Input.GetMouseButtonDown(0))
        {
            
            if (leftCollider.GetComponent<Collider2D>().bounds.Contains(Input.mousePosition)) // เริ่มคลิกในพื้นที่ด้านซ้ายของผ้าห่ม
            {
                hand.SetActive(true);
                holding = true;
            }
        }
        if (Input.GetMouseButton(0)) // ขณะคลิกค้าง ย้ายรูปมือตามเมาส์
        {
            hand.transform.position = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (rightCollider.GetComponent<Collider2D>().bounds.Contains(Input.mousePosition) && holding) // ปล่อยคลิกในพื้นที่ด้านซ้ายของผ้าห่ม 
            {
                foldGame0.SetActive(false);
                foldGame1.SetActive(true);
                AudioManager.Instance.PlayOneShot(correctSound);
                DisplayNextDialogue();
            }
            hand.SetActive(false);

            holding = false;
        }
    }

    private void FoldingGame1()
    {
        if (!hasCompleted)
            PlayVO(3);
        if (Input.GetMouseButtonDown(0))
        {
            if (upCollider.GetComponent<Collider2D>().bounds.Contains(Input.mousePosition)) // เริ่มคลิกในพื้นที่ด้านบนของผ้าห่ม
            {
                hand.SetActive(true);
                holding = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            hand.transform.position = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (downCollider.GetComponent<Collider2D>().bounds.Contains(Input.mousePosition) && holding) // ปล่อยคลิกในพื้นที่ด้านล่างของผ้าห่ม 
            {
                foldGame1.SetActive(false);
                foldGame2.SetActive(true);
                StartCoroutine(FinishFoldCanvas());
                CounterManager.Instance.SetEnableCounter(false);
                this.transform.GetChild(0).GetChild(5).gameObject.SetActive(true); //รูปผ้าห่มพับเสร็จ (BG)
                this.transform.GetChild(0).GetChild(6).gameObject.SetActive(false); //ตัวละครเด็กยืนงง (BG)
                this.transform.GetChild(0).GetChild(7).gameObject.SetActive(true); //ตัวละครเด็กดีใจ (BG)
                if (ModuleManager.Instance.characterGender)
                    this.transform.GetChild(0).GetChild(7).gameObject.GetComponent<Animator>().SetBool("Gender", true); //ตัวละครเด็กดีใจ (BG)
                else
                    this.transform.GetChild(0).GetChild(7).gameObject.GetComponent<Animator>().SetBool("Gender", false); //ตัวละครเด็กดีใจ (BG)
                this.transform.GetChild(0).GetChild(8).gameObject.SetActive(false); //รูปความคิด อยากให้เตียงเรียบร้อย (BG)
                this.transform.GetChild(0).GetChild(9).gameObject.SetActive(true); //รูปความคิด ยกนิ้วโป้ง (BG)
                //time.SetActive(false);
                AudioManager.Instance.PlayOneShot(correctSound);
                if (!hasCompleted)
                    PlayVO(4);
                DisplayNextDialogue();
            }
            hand.SetActive(false);

            holding = false;
        }
    }

    public override void ResetModule()
    {
        TriggerDialogue();
        //OnEnable();
        foldGame0 = this.transform.GetChild(1).GetChild(2).gameObject;
        leftCollider = foldGame0.transform.GetChild(0).GetChild(0).gameObject;
        rightCollider = foldGame0.transform.GetChild(0).GetChild(1).gameObject;

        foldGame1 = this.transform.GetChild(1).GetChild(3).gameObject;
        upCollider = foldGame1.transform.GetChild(0).GetChild(0).gameObject;
        downCollider = foldGame1.transform.GetChild(0).GetChild(1).gameObject;

        foldGame2 = this.transform.GetChild(1).GetChild(4).gameObject;

        hand = this.transform.GetChild(1).GetChild(5).gameObject;

        if (ModuleManager.Instance.characterGender)
            this.transform.GetChild(0).GetChild(6).gameObject.GetComponent<Image>().sprite = boy;
        else
            this.transform.GetChild(0).GetChild(6).gameObject.GetComponent<Image>().sprite = girl;



        foldGame0.SetActive(false);
        foldGame1.SetActive(false);
        foldGame2.SetActive(false);
        hand.SetActive(false);
        this.transform.GetChild(1).GetChild(0).gameObject.SetActive(true); 
        this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = false; // first blanket
        LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false; // hand pointing to first blanket

        this.transform.GetChild(0).GetChild(5).gameObject.SetActive(false); // blanket success
        this.transform.GetChild(0).GetChild(6).gameObject.SetActive(true); // character
        this.transform.GetChild(0).GetChild(7).gameObject.SetActive(false); // character dance
        this.transform.GetChild(0).GetChild(8).gameObject.SetActive(false); // thinking
        this.transform.GetChild(0).GetChild(9).gameObject.SetActive(false); // thinking thumbs up

        if (hasCompleted)
        {
            foldGame0.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false; // line hint
            foldGame0.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false; // arrow hint
            foldGame1.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false; // line hint
            foldGame1.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false; // arrow hint
        }

        firstVO = false;
        ProgressBarManager.Instance.SetEnableProgressBar(true);
    }

    public override void GetHelp()
    {
        foldGame0.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true; // line hint
        foldGame0.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true; // arrow hint
        foldGame1.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true; // line hint
        foldGame1.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true; // arrow hint
        LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true; // hand pointing to first blanket
    }

    IEnumerator FinishFoldCanvas()
    {
        yield return new WaitForSeconds(3);
        foldGame2.GetComponent<Animator>().SetTrigger("Close");
        yield return new WaitForSeconds(2);
        DisplayNextDialogue();
        foldGame2.SetActive(false);
        if (!hasCompleted)
            AudioManager.Instance.PlayVoiceOver(VoiceOver[5],checkVO[5]);
        checkVO[5] = true;
        if (!hasCompleted)
            SetCompleteWithResult(1);
        else
        {
            if (foldGame0.transform.GetChild(1).gameObject.GetComponent<Image>().enabled) // line hint (เช็คว่า hint เปิดอยู่หรือเปล่า)
                SetCompleteWithResult(2); //input star here
            else
                SetCompleteWithResult(3);
        }
    }

    public void ButtonClickSound(AudioClip clip)
    {
        AudioManager.Instance.PlayOneShot(clip);
    }

    IEnumerator PlayFirstVO()
    {
        AudioManager.Instance.PlayVoiceOver(VoiceOver[0], checkVO[0]);
        checkVO[0] = true;
        yield return new WaitForSeconds(VoiceOver[0].length);
        this.transform.GetChild(0).GetChild(8).gameObject.SetActive(true); // thinking
        DisplayNextDialogue();
        if (!hasCompleted)
        {
            PlayVO(1);
            yield return new WaitForSeconds(VoiceOver[1].length);
            DisplayNextDialogue();
            LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true; //hand
        }
        else
        {
            PlayVO(6);
            yield return new WaitForSeconds(VoiceOver[6].length);
            DisplayNextDialogue();
        }
        this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Button>().interactable = true; // blanket
        CounterManager.Instance.SetEnableCounter(true);
    }
}
