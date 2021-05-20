using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushTeethModule : Module
{
    public GameObject BrushTeethGame0; //object Game 0 : บีบยาสีฟันใส่แปรง
    public GameObject BrushTeethGame1; //object Game 1 : แปรงฟัน
    public bool firstTrig; // check การบีบยาสีฟันใส่แปรงจุดแรก (โคนแปรง)
    public Vector3 initPos; // Position เริ่มต้นของรูปที่กำหนด
    public BrushUpDown brushUpDown; // Controller การแปรงฟันขึ้นลง (Game 1)
    public bool completeBrushing; // check การแปรงฟันเสร็จ รอบ้วนน้ำ (Game 1 complete)
    public Animator BOY2;

    public List<Sprite> boySprite = new List<Sprite>();
    public List<Sprite> girlSprite = new List<Sprite>();

    private bool holding = false; //ตรวจสอบว่าคลิกค้างอยู่ (โดยเริ่มคลิกในพื้นที่ที่กำหนดไว้)
    private bool secondTrig = false; // check การบีบยาสีฟันใส่แปรงจุดที่ 2 (ปลายแปรง)
    private Vector3 deltaClick; // Position ของเมาส์เมื่อคลิก - Position ของรูปที่ลาก

    private bool dialogCheckforToothPaste = false;
    private bool firstVO = false;


    public override void GetHelp()
    {
        LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true; // รูปความคิดว่าอยากแปรงฟัน ในฉากแรก
        LayerInteractable[0].transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true; // รูปมือ ชี้ประตู ในฉากแรก
        LayerInteractable[1].transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true; // รูปมือ ชี้แปรงบนซิงค์ ในฉากสอง
        BrushTeethGame0.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true; // ลูกศรโค้ง ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame0.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true; // ลูกศรตรง ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame1.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true; // ลูกศรโค้ง ในเกมแปรงฟัน (Game1)
        BrushTeethGame1.transform.GetChild(3).gameObject.GetComponent<Image>().enabled = true; // ลูกศรขึ้น ในเกมแปรงฟัน (Game1)
        BrushTeethGame1.transform.GetChild(4).gameObject.GetComponent<Image>().enabled = true; // ลูกศรลง ในเกมแปรงฟัน (Game1)
        LayerBackground[1].transform.GetChild(10).gameObject.GetComponent<Image>().enabled = true; // รูปมือชี้แก้วบ้วนน้ำ ฉากสุดท้าย
    }

    public override void PlayingModule()
    {
        if (!firstVO)
        {
            firstVO = true;
            StartCoroutine(PlayFirstVO());
        }
        if (BrushTeethGame0.activeSelf)
        {
            if (initPos == new Vector3(0, 0, 0)) // ถ้ายังไม่ได้ Set InitPos ให้ InitPos เท่ากับตำแหน่งของรูปหลอดยาสีฟัน
            {
                initPos = BrushTeethGame0.transform.GetChild(5).transform.position; // Position ของรูปหลอดยาสีฟัน
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (BrushTeethGame0.transform.GetChild(5).GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เริ่มคลิกใน Collider ของรูปหลอดยาสีฟัน
                {
                    deltaClick = Input.mousePosition - BrushTeethGame0.transform.GetChild(5).transform.position; // เก็บ DeltaClick ไว้ย้ายรูปตาม Position ของเมาส์
                    holding = true;
                }
            }
            else if (Input.GetMouseButton(0) && holding) // ขณะคลิกค้าง ย้ายรูปหลอดยาสีฟันตามเมาส์
            {
                BrushTeethGame0.transform.GetChild(5).transform.position = Input.mousePosition - deltaClick;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                BrushTeethGame0.transform.GetChild(5).transform.position = initPos; // ย้ายรูปหลอดยาสีฟันไปตำแหน่งเดิม
                holding = false;
                if (!BrushTeethGame0.transform.GetChild(4).gameObject.activeSelf) // ถ้ารูปยาสีฟันครึ่งสุดท้ายยังไม่ active (บีบไม่เสร็จ) ให้ทำการ Set ฉากเป็นหมือนเดิม
                {
                    ClickBrush();
                }
                if (secondTrig) // ถ้าบีบยาสีฟันใส่แปรงจุดที่ 2 (ปลายแปรง) แล้ว
                {
                    initPos = new Vector3(0, 0, 0);
                    BrushTeethGame0.SetActive(false); 
                    BrushTeethGame0.transform.GetChild(2).gameObject.SetActive(true); // ลูกศรตรง ในเกมบีบยาสีฟัน (Game0)
                    BrushTeethGame1.transform.GetChild(2).gameObject.SetActive(true); // ลูกศรโค้ง ในเกมแปรงฟัน (Game1)
                    brushUpDown.firstCome = false;
                    BrushTeethGame1.SetActive(true);
                }
            }
        }

        else if (BrushTeethGame1.activeSelf)
        {
            if (!hasCompleted)
                PlayVO(4);
            if (initPos == new Vector3(0, 0, 0)) // ถ้ายังไม่ได้ Set InitPos ให้ InitPos เท่ากับตำแหน่งของรูปแปรงสีฟัน
            {
                initPos = BrushTeethGame1.transform.GetChild(6).transform.position; // Position ของรูปแปรงสีฟัน
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (BrushTeethGame1.transform.GetChild(6).GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เริ่มคลิกใน Collider ของรูปแปรงสีฟัน
                {
                    deltaClick = Input.mousePosition - BrushTeethGame1.transform.GetChild(6).transform.position; // เก็บ DeltaClick ไว้ย้ายรูปตาม Position ของเมาส์
                    holding = true;
                }
            }
            else if (Input.GetMouseButton(0) && holding) // ขณะคลิกค้าง ย้ายรูปแปรงสีฟันตามเมาส์
            {
                BrushTeethGame1.transform.GetChild(6).transform.position = Input.mousePosition - deltaClick;
            }
            else if (Input.GetMouseButtonUp(0)) // เมื่อปล่อยเมาส์ ให้ย้ายตำแหน่งรูปแปรงมาที่เดิม
            {
                BrushTeethGame1.transform.GetChild(6).transform.position = initPos;
                holding = false;
            }
        }

        else if (completeBrushing)
        {
            LayerInteractable[1].gameObject.SetActive(false);
            LayerInteractable[2].gameObject.SetActive(true);
            if (!hasCompleted)
                PlayVO(5);
        }

        if (complete)
        {
            SetCompleteWithResult(3); //input star here
            //ModuleManager.Instance.LayerResult.SetActive(true);
            complete = false;
        }
    }

    public void ClickDoor() // ทำงานเมื่อกด Button ประตู
    {
        LayerBackground[0].gameObject.SetActive(false);
        LayerBackground[1].gameObject.SetActive(true);
        LayerInteractable[0].gameObject.SetActive(false);
        LayerInteractable[1].gameObject.SetActive(true);
        LayerInteractable[1].transform.GetChild(0).gameObject.SetActive(true);  // รูปมือ ชี้แปรงบนซิงค์ ในฉากสอง
        LayerInteractable[1].transform.GetChild(1).gameObject.SetActive(true);  // รูป แปรงบนซิงค์ ในฉากสอง
        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(1);
    }

    public void ClickBrush() // ทำงานเมื่อกดรูปแปรงบนซิงค์ 
    {
        BrushTeethGame0.transform.GetChild(1).gameObject.SetActive(true); // ลูกศรโค้ง ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame0.transform.GetChild(2).gameObject.SetActive(true); // ลูกศรตรง ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame0.transform.GetChild(3).gameObject.SetActive(false); // ยาสีฟันบนแปรง ครึ่งแรก ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame0.transform.GetChild(4).gameObject.SetActive(false); // ยาสีฟันบนแปรง ครึ่งหลัง ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame0.SetActive(true);
        LayerBackground[1].transform.GetChild(1).gameObject.SetActive(false); // รูปเด็ก ปากไม่มีฟอง
        LayerBackground[1].transform.GetChild(2).gameObject.SetActive(true); // รูปเด็ก ปากมีฟอง
        LayerBackground[1].transform.GetChild(4).gameObject.SetActive(false); // แก้วน้ำบ้วนปาก
        LayerBackground[1].transform.GetChild(8).gameObject.SetActive(false); // รูปความคิดอยากแปรงฟัน
        LayerBackground[1].transform.GetChild(9).gameObject.SetActive(true); // รูปความคิดอยากบ้วนปาก
        LayerBackground[1].transform.GetChild(10).gameObject.SetActive(true); // รูปมือชี้แก้ว

        LayerInteractable[1].transform.GetChild(0).gameObject.SetActive(false); // รูปมือ ชี้แปรงบนซิงค์ ในฉากสอง
        LayerInteractable[1].transform.GetChild(1).gameObject.SetActive(false); // รูป แปรงบนซิงค์ ในฉากสอง
        firstTrig = false;
        secondTrig = false;

        if (!hasCompleted)
            PlayVO(2);
        //DisplayNextDialogue();
    }
    public void FirstTriger() // เมื่อหัวหลอดชนขนแปรงจุดแรก
    {
        BrushTeethGame0.transform.GetChild(1).gameObject.SetActive(false); // ลูกศรโค้ง ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame0.transform.GetChild(2).gameObject.SetActive(true); // ลูกศรตรง ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame0.transform.GetChild(3).gameObject.SetActive(true); // ยาสีฟันบนแปรง ครึ่งแรก ในเกมบีบยาสีฟัน (Game0)
        firstTrig = true;
        secondTrig = false;
    }

    public void SecondTrigger() // เมื่อหัวหลอดชนขนแปรงจุดที่สอง
    {
        BrushTeethGame0.transform.GetChild(4).gameObject.SetActive(true); // ยาสีฟันบนแปรง ครึ่งหลัง ในเกมบีบยาสีฟัน (Game0)
        BrushTeethGame0.transform.GetChild(2).gameObject.SetActive(true); // ลูกศรตรง ในเกมบีบยาสีฟัน (Game0)
        secondTrig = true;
        if(firstTrig&& secondTrig&&!dialogCheckforToothPaste)
        {
            DisplayNextDialogue();
            AudioManager.Instance.PlayOneShot(correctSound);
            dialogCheckforToothPaste = true;
            if (!hasCompleted)
                PlayVO(3);
        }
    }

    public override void ResetModule()
    {
        firstVO = false;
        TriggerDialogue();
        brushUpDown.resetDialogue();

        LayerBackground[0].gameObject.SetActive(true);
        if (ModuleManager.Instance.characterGender) // change sprite character gender
        {
            LayerBackground[0].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = boySprite[0]; // เด็กยืนหน้าห้องน้ำ
            LayerBackground[1].transform.GetChild(1).gameObject.GetComponent<Image>().sprite = boySprite[1]; // รูปเด็ก ปากไม่มีฟอง
            LayerBackground[1].transform.GetChild(2).gameObject.GetComponent<Image>().sprite = boySprite[2]; // รูปเด็ก ปากมีฟอง
            LayerInteractable[1].transform.GetChild(3).GetChild(0).gameObject.GetComponent<Image>().sprite = boySprite[3]; // รูปฟันเด็ก
        }
        else
        {
            LayerBackground[0].transform.GetChild(4).gameObject.GetComponent<Image>().sprite = girlSprite[0]; // เด็กยืนหน้าห้องน้ำ
            LayerBackground[1].transform.GetChild(1).gameObject.GetComponent<Image>().sprite = girlSprite[1]; // รูปเด็ก ปากไม่มีฟอง
            LayerBackground[1].transform.GetChild(2).gameObject.GetComponent<Image>().sprite = girlSprite[2]; // รูปเด็ก ปากมีฟอง 
            LayerInteractable[1].transform.GetChild(3).GetChild(0).gameObject.GetComponent<Image>().sprite = girlSprite[3]; // รูปฟันเด็ก
        }
        LayerBackground[1].gameObject.SetActive(false);
        LayerInteractable[0].gameObject.SetActive(true);
        LayerInteractable[1].gameObject.SetActive(false);
        LayerInteractable[2].gameObject.SetActive(false);

        BrushTeethGame0.SetActive(false);
        BrushTeethGame1.SetActive(false);
        holding = false;
        secondTrig = false;
        completeBrushing = false;
        initPos = new Vector3(0, 0, 0);
        firstTrig = false;
        dialogCheckforToothPaste = false;

        BOY2.enabled = false; //disable dancing kid

        LayerBackground[1].transform.GetChild(1).gameObject.SetActive(true); // รูปเด็ก ปากไม่มีฟอง
        LayerBackground[1].transform.GetChild(2).gameObject.SetActive(false); // รูปเด็ก ปากมีฟอง
        LayerBackground[1].transform.GetChild(4).gameObject.SetActive(true); // แก้วน้ำบ้วนปาก
        LayerBackground[1].transform.GetChild(8).gameObject.SetActive(true); // รูปความคิดอยากแปรงฟัน
        LayerBackground[1].transform.GetChild(9).gameObject.SetActive(false); // รูปความคิดอยากบ้วนปาก
        LayerBackground[1].transform.GetChild(10).gameObject.SetActive(false); // รูปมือชี้แก้ว
        LayerBackground[1].transform.GetChild(11).gameObject.SetActive(false); // รูปความคิดยกนิ้วโป้ง


        LayerInteractable[0].transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false; // door
        LayerInteractable[2].transform.GetChild(0).gameObject.SetActive(true); // glass button
        //LayerBackground[1].transform.GetChild(2).gameObject.GetComponent<Image>().sprite = BOY2StartSprite; // reset boy2 to first frame
        LayerBackground[1].transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(false); //sparkle 0-3
        LayerBackground[1].transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(false);
        LayerBackground[1].transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(false);
        LayerBackground[1].transform.GetChild(2).transform.GetChild(3).gameObject.SetActive(false);
        LayerInteractable[0].transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false; // มือชี้ประตู

        if (hasCompleted)
        {
            //LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false;
            LayerInteractable[1].transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false; // มือชี้แปรงสีฟัน
            BrushTeethGame0.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false; // ลูกศรโค้ง ในเกมบีบยาสีฟัน (Game0)
            BrushTeethGame0.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false; // ลูกศรตรง ในเกมบีบยาสีฟัน (Game0)
            BrushTeethGame1.transform.GetChild(2).gameObject.GetComponent<Image>().enabled = false; // ลูกศรโค้ง ในเกมแปรงฟัน (Game1)
            BrushTeethGame1.transform.GetChild(3).gameObject.GetComponent<Image>().enabled = false; // ลูกศรขึ้น ในเกมแปรงฟัน (Game1)
            BrushTeethGame1.transform.GetChild(4).gameObject.GetComponent<Image>().enabled = false; // ลูกศรลง ในเกมแปรงฟัน (Game1)
            LayerBackground[1].transform.GetChild(10).gameObject.GetComponent<Image>().enabled = false; // รูปมือชี้แก้ว
        }
       
        ProgressBarManager.Instance.SetEnableProgressBar(true);
    }

    public void onClickGlass()
    {
        AudioManager.Instance.PlayOneShot(correctSound);
        if (!hasCompleted)
            PlayVO(6);
        DisplayNextDialogue();
        CounterManager.Instance.SetEnableCounter(false);
        AudioManager.Instance.WaitandPlaySound(2.6f,SFX[0]);
        StartCoroutine(FinishFoldCanvas());
        LayerBackground[1].transform.GetChild(2).gameObject.GetComponent<Animator>().enabled = true; //sparkle
        if (ModuleManager.Instance.characterGender) // change sprite character gender
            LayerBackground[1].transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Gender", true);
        else
            LayerBackground[1].transform.GetChild(2).gameObject.GetComponent<Animator>().SetBool("Gender", false);
    }

    IEnumerator FinishFoldCanvas()
    {
        yield return new WaitForSeconds(3f);
        DisplayNextDialogue();
        float totaltime = 0f;
        AnimationClip[] clips = BOY2.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            totaltime = totaltime + clip.length;
        }
        yield return new WaitForSeconds(2.5f);
        PlayVO(9);
        DisplayNextDialogue();

        yield return new WaitForSeconds(VoiceOver[9].length);
        DisplayNextDialogue();
        if (!hasCompleted)
           PlayVO(7);
        if (!hasCompleted)
            SetCompleteWithResult(1);
        else
        {
            if (BrushTeethGame0.transform.GetChild(2).gameObject.GetComponent<Image>().enabled) // ลูกศรตรง ในเกมบีบยาสีฟัน (Game0)
                SetCompleteWithResult(2);
            else
                SetCompleteWithResult(3);
        }
        yield return new WaitForSeconds(1f);


    }

    public void ButtonClickSound(AudioClip clip)
    {
        AudioManager.Instance.PlayOneShot(clip);
    }

    IEnumerator PlayFirstVO()
    {
        if (!hasCompleted)
        {
            PlayVO(0);
            yield return new WaitForSeconds(VoiceOver[0].length);
            LayerInteractable[0].transform.GetChild(2).gameObject.GetComponent<Image>().enabled = true; // มือชี้ประตู
        }
        else
        {
            PlayVO(8);
            DialogueManager.Instance.ForceSetText("เราต้องไปทำอะไรนะ");
            yield return new WaitForSeconds(3f);
            DialogueManager.Instance.ForceSetText("ใช่แล้ว ไปแปรงฟันกัน");
            yield return new WaitForSeconds(VoiceOver[8].length-3f);
        }
        LayerInteractable[0].transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true; // door
        CounterManager.Instance.SetEnableCounter(true);
    }


}
