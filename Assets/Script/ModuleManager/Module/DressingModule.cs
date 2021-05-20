using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DressingModule : Module
{

    public GameObject shirt; // รูปเสื้อ
    public GameObject pants; // รูปกระโปง/กางเกง
    public Vector3 initPosShirt; // Position เริ่มต้นของเสื้อ
    public Vector3 initPosPants; // Position เริ่มต้นของกางเกง/กระโปรง
    public GameObject mainArrow_1; // ลูกสรชี้ให้ใส่เสื้อ
    public GameObject mainArrow_2; // ลูกสรชี้ให้ใส่กางเกง/กระโปรง


    public bool holding; //ตรวจสอบว่าคลิกค้างอยู่ (โดยเริ่มคลิกในพื้นที่ที่กำหนดไว้)
    public bool tShirtTrigger; // Check เสื้อ ว่าอยู่ในพื้นที่สวมใส่
    public bool pantsTrigger; // Check ว่ากางเกง/กระโปรง ว่าอยู่ในพื้นที่สวมใส่

    public GameObject handPushing_Tshirt; // รูปมือ เมื่อลากตอนใส่เสื้อ
    public GameObject handPushing_Pants; // รูปมือ เมื่อลากตอนใส่กางเกง/กระโปรง

    public Image boyPushingTshirt; //รูปเด็กใส่เสื้อ
    public Image boyPushingPants; //รูปเด็กใส่กางเกง/กระโปรง
    public Image bgKid; // รูปเด็กยืนรอแต่งตัว (แก้ผ้า -> แต่งเสร็จ)

    public GameObject _1_Up; // Collider บน (ใส่เสื้อแขนฝั่งซ้าย)
    public GameObject _1_Down; // Collider ล่าง (ใส่เสื้อแขนฝั่งซ้าย)
    public GameObject _1_Arrow; // ลูกศร (ใส่เสื้อแขนฝั่งซ้าย)

    public GameObject _2_Up; // Collider บน (ใส่เสื้อแขนฝั่งขวา)
    public GameObject _2_Down; // Collider ล่าง (ใส่เสื้อแขนฝั่งขวา)
    public GameObject _2_Arrow; // ลูกศร (ใส่เสื้อแขนฝั่งขวา)

    public GameObject _3_Up; // Collider บน (ดึงเสื้อขึ้น)
    public GameObject _3_Down; // Collider ล่าง (ดึงเสื้อขึ้น)
    public GameObject _3_Arrow; // ลูกศร (ดึงเสื้อขึ้น)

    public GameObject poongPing_Tshirt; // เอฟเฟกต์ปุ๊งปิ๊ง

    public GameObject _4_Up; // Collider บน (ใส่กางเกง/กระโปรงขาฝั่งซ้าย)
    public GameObject _4_Down; // Collider ล่าง (ใส่กางเกง/กระโปรงขาฝั่งซ้าย)
    public GameObject _4_Arrow; // ลูกศร (ใส่กางเกง/กระโปรงขาฝั่งซ้าย)

    public GameObject _5_Up; // Collider บน (ใส่กางเกง/กระโปรงขาฝั่งขวา)
    public GameObject _5_Down; // Collider ล่าง (ใส่กางเกง/กระโปรงขาฝั่งขวา)
    public GameObject _5_Arrow; // ลูกศร (ใส่กางเกง/กระโปรงขาฝั่งขวา)

    public GameObject _6_Up; // Collider บน (ดึงกางเกง/กระโปรงขึ้น)
    public GameObject _6_Down; // Collider ล่าง (ดึงกางเกง/กระโปรงขึ้น)
    public GameObject _6_Arrow; // ลูกศร (ดึงกางเกง/กระโปรงขึ้น)

    public GameObject poongPing_Pants; // เอฟเฟกต์ปุ๊งปิ๊ง

    private bool _1_pass = false; // ผ่านใส่เสื้อแขนฝั่งซ้าย
    private bool _2_pass = false; // ผ่านใส่เสื้อแขนฝั่งขวา
    private bool _3_pass = false; // ผ่านดึงเสื้อขึ้น
    private bool _4_pass = false; // ผ่านใส่ขาข้างฝั่งซ้าย
    private bool _5_pass = false; // ผ่านใส่ขาข้างฝั่งขวา
    private bool _6_pass = false; // ผ่านดึงกางเกง/กระโปรงขึ้น

    private Vector3 deltaClick; // Position ของเมาส์เมื่อคลิก - Position ของรูปที่ลาก
    private string item = "empty"; // Object หรือ Item ที่กำลังลาก

    public List<Sprite> boy = new List<Sprite>();
    public List<Sprite> girl = new List<Sprite>();

    private bool firstVO = false;
    //spite order
    //0 = pajamas
    //1 = pre-shirt
    //2 = shirt1
    //3 = shirt2
    //4 = shirt3
    //5 = shirtPass
    //6 = pre-pants
    //7 = pants1
    //8 = pant2
    //9 = pantPass
    //10 = naked
    //11 = finish
    //12 = pantsSprite

    public override void ResetModule()
    {
        CounterManager.Instance.SetEnableCounter(true);

        firstVO = false;

        handPushing_Tshirt.SetActive(false);
        handPushing_Pants.SetActive(false);

        if (ModuleManager.Instance.characterGender)
        {
            LayerBackground[0].transform.GetChild(2).gameObject.GetComponent<Image>().sprite = boy[0]; // รูปเด็กยืนในชุดนอน
            LayerInteractable[0].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = boy[13]; // ตู้เสื้อผ้า
            LayerBackground[1].transform.GetChild(6).gameObject.GetComponent<Image>().sprite = boy[14]; // รูปความคิด อยากใส่กางเกง/กระโปรง
            boyPushingTshirt.sprite = boy[1];
            bgKid.sprite = boy[10];
            pants.GetComponent<Image>().sprite = boy[12];
            sentences[1] = "หยิบเสื้อให้อิ่มบุญใส่";
            sentences[5] = "ดึงชายเสื้อลง";
            sentences[6] = "หยิบกางเกงให้อิ่มบุญใส่";
            sentences[7] = "ใส่ขาซ้ายเข้าไปในขากางเกง";
            sentences[8] = "ใส่ขาขวาเข้าไปในขากางเกง";
            sentences[9] = "ดึงขอบกางเกงขึ้น";
            sentences[10] = "จัดกางเกงให้เรียบร้อย";
        }
        else
        {
            LayerBackground[0].transform.GetChild(2).gameObject.GetComponent<Image>().sprite = girl[0]; // รูปเด็กยืนในชุดนอน
            LayerInteractable[0].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = girl[13]; // ตู้เสื้อผ้า
            LayerBackground[1].transform.GetChild(6).gameObject.GetComponent<Image>().sprite = girl[14]; // รูปความคิด อยากใส่กางเกง/กระโปรง
            boyPushingTshirt.sprite = girl[1];
            bgKid.sprite = girl[10];
            pants.GetComponent<Image>().sprite = girl[12];
            sentences[1] = "หยิบเสื้อให้อุ่นใจใส่";
            sentences[5] = "ดึงชายเสื้อลง";
            sentences[6] = "หยิบกระโปรงให้อุ่นใจใส่";
            sentences[7] = "ใส่ขาซ้ายเข้าไปในกระโปรง";
            sentences[8] = "ใส่ขาขวาเข้าไปในกระโปรง";
            sentences[9] = "ดึงขอบกระโปรงขึ้น";
            sentences[10] = "จัดกระโปรงให้เรียบร้อย";
            
        }

        TriggerDialogue();

        LayerInteractable[0].transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false; // wardrobe button

        shirt.SetActive(true);
        pants.SetActive(true);
        mainArrow_1.SetActive(true);
        mainArrow_2.SetActive(false);

        _1_pass = false;
        _2_pass = false;
        _3_pass = false;
        _4_pass = false;
        _5_pass = false;
        _6_pass = false;

        _1_Up.SetActive(true);
        _1_Down.SetActive(true);
        _1_Arrow.SetActive(true);

        _2_Up.SetActive(false);
        _2_Down.SetActive(false);
        _2_Arrow.SetActive(false);

        _3_Up.SetActive(false);
        _3_Down.SetActive(false);
        _3_Arrow.SetActive(false);

        _4_Up.SetActive(true);
        _4_Down.SetActive(true);
        _4_Arrow.SetActive(true);

        _5_Up.SetActive(false);
        _5_Down.SetActive(false);
        _5_Arrow.SetActive(false);

        _6_Up.SetActive(false);
        _6_Down.SetActive(false);
        _6_Arrow.SetActive(false);

        poongPing_Tshirt.SetActive(false);

        poongPing_Pants.SetActive(false);

        holding = false;
        tShirtTrigger = false;
        LayerBackground[0].gameObject.SetActive(true);
        LayerInteractable[0].gameObject.SetActive(true);
        LayerBackground[1].gameObject.SetActive(false);
        LayerInteractable[1].gameObject.SetActive(false);
        LayerBackground[2].gameObject.SetActive(false);
        LayerInteractable[2].gameObject.SetActive(false);
        LayerBackground[3].gameObject.SetActive(false);
        LayerInteractable[3].gameObject.SetActive(false);

        LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false; // มือชี้ตู้

        LayerBackground[1].transform.GetChild(2).gameObject.SetActive(true); // white
        LayerBackground[1].transform.GetChild(3).gameObject.SetActive(false); // thinking
        LayerBackground[1].transform.GetChild(4).gameObject.SetActive(false); // star
        LayerBackground[1].transform.GetChild(5).gameObject.SetActive(true); // thinking shirt
        LayerBackground[1].transform.GetChild(6).gameObject.SetActive(false); // thinking pants

        if (hasCompleted)
        {
            mainArrow_1.GetComponent<Image>().enabled = false;
            mainArrow_2.GetComponent<Image>().enabled = false;
            _1_Arrow.GetComponent<Image>().enabled = false;
            _2_Arrow.GetComponent<Image>().enabled = false;
            _3_Arrow.GetComponent<Image>().enabled = false;
            _4_Arrow.GetComponent<Image>().enabled = false;
            _5_Arrow.GetComponent<Image>().enabled = false;
            _6_Arrow.GetComponent<Image>().enabled = false;
            LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = false; // hand
        }
    }

    public override void GetHelp()
    {
        mainArrow_1.GetComponent<Image>().enabled = true;
        mainArrow_2.GetComponent<Image>().enabled = true;
        _1_Arrow.GetComponent<Image>().enabled = true;
        _2_Arrow.GetComponent<Image>().enabled = true;
        _3_Arrow.GetComponent<Image>().enabled = true;
        _4_Arrow.GetComponent<Image>().enabled = true;
        _5_Arrow.GetComponent<Image>().enabled = true;
        _6_Arrow.GetComponent<Image>().enabled = true;
        LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true; // hand
    }

    public void ClickWardrobe() // เมื่อกดตู้เสื้อผ้า
    {
        LayerBackground[0].gameObject.SetActive(false);
        LayerInteractable[0].gameObject.SetActive(false);
        LayerBackground[1].gameObject.SetActive(true);
        LayerInteractable[1].gameObject.SetActive(true);
        initPosShirt = shirt.transform.localPosition;
        initPosPants = pants.transform.localPosition;
        DisplayNextDialogue();
        if (!hasCompleted) 
            PlayVO(1);
    }

    public void PushTShirt() // เมื่อลากเสื้อมาใส่
    {
        LayerBackground[1].gameObject.SetActive(false);
        LayerInteractable[1].gameObject.SetActive(false);
        LayerBackground[2].gameObject.SetActive(true);
        LayerInteractable[2].gameObject.SetActive(true);
        DisplayNextDialogue();
        if (!hasCompleted) 
            PlayVO(2);
        AudioManager.Instance.PlayOneShot(correctSound);
    }

    public void PassTshirt_1_() // เมื่อใส่แขนฝั่งซ้ายแล้ว
    {
        if (ModuleManager.Instance.characterGender)
            boyPushingTshirt.sprite = boy[2];
        else
            boyPushingTshirt.sprite = girl[2];
        _1_Up.SetActive(false);
        _1_Down.SetActive(false);
        _1_Arrow.SetActive(false);
        _2_Up.SetActive(true);
        _2_Down.SetActive(true);
        _2_Arrow.SetActive(true);
        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(3);
        AudioManager.Instance.PlayOneShot(correctSound);
    }

    public void PassTshirt_2_() // เมื่อใส่แขนฝั่งขวาแล้ว
    {
        if (ModuleManager.Instance.characterGender)
            boyPushingTshirt.sprite = boy[3];
        else
            boyPushingTshirt.sprite = girl[3];
        _2_Up.SetActive(false);
        _2_Down.SetActive(false);
        _2_Arrow.SetActive(false);
        _3_Up.SetActive(true);
        _3_Down.SetActive(true);
        _3_Arrow.SetActive(true);
        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(4);
        AudioManager.Instance.PlayOneShot(correctSound);
    }

    public void PassTshirt_3_() // เมื่อใส่ดึงเสื้อขึ้นแล้ว
    {
        if (ModuleManager.Instance.characterGender)
            boyPushingTshirt.sprite = boy[4];
        else
            boyPushingTshirt.sprite = girl[4];
        _3_Up.SetActive(false);
        _3_Down.SetActive(false);
        _3_Arrow.SetActive(false);
        poongPing_Tshirt.SetActive(true);
        AudioManager.Instance.PlayOneShot(correctSound);
        StartCoroutine(PassTShirt());
        
    }

    IEnumerator PassTShirt()
    {
        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(5);
        yield return new WaitForSeconds(VoiceOver[5].length);

        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(6);

        if (ModuleManager.Instance.characterGender)
            bgKid.sprite = boy[5];
        else
            bgKid.sprite = girl[5];

        
        shirt.SetActive(false);
        
        mainArrow_1.SetActive(false);
        mainArrow_2.SetActive(true);

        LayerBackground[1].gameObject.gameObject.SetActive(true);
        LayerInteractable[1].gameObject.SetActive(true);

        LayerBackground[1].transform.GetChild(5).gameObject.SetActive(false); // thinking shirt
        LayerBackground[1].transform.GetChild(6).gameObject.SetActive(true); // thinking pants

        pants.transform.position = initPosPants;

        LayerBackground[2].gameObject.gameObject.SetActive(false);
        LayerInteractable[2].gameObject.gameObject.SetActive(false);
    }

    public void PushPants() // เมื่อลากกางเกง/กระโปรงมาใส่
    {
        if (ModuleManager.Instance.characterGender)
            boyPushingPants.sprite = boy[6];
        else
            boyPushingPants.sprite = girl[6];
        LayerBackground[1].gameObject.SetActive(false);
        LayerInteractable[1].gameObject.SetActive(false);
        LayerBackground[3].gameObject.SetActive(true);
        LayerInteractable[3].gameObject.SetActive(true);
        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(7);
        AudioManager.Instance.PlayOneShot(correctSound);
    }

    public void PassPants_1_() // เมื่อใส่ขาฝั่งซ้ายแล้ว
    {
        if (ModuleManager.Instance.characterGender)
            boyPushingPants.sprite = boy[7];
        else
            boyPushingPants.sprite = girl[7];
        _4_Up.SetActive(false);
        _4_Down.SetActive(false);
        _4_Arrow.SetActive(false);
        _5_Up.SetActive(true);
        _5_Down.SetActive(true);
        _5_Arrow.SetActive(true);
        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(8);
        AudioManager.Instance.PlayOneShot(correctSound);
    }

    public void PassPants_2_() // เมื่อใส่ขาฝั่งขวาแล้ว
    {
        if (ModuleManager.Instance.characterGender)
            boyPushingPants.sprite = boy[8];
        else
            boyPushingPants.sprite = girl[8];
        _5_Up.SetActive(false);
        _5_Down.SetActive(false);
        _5_Arrow.SetActive(false);
        _6_Up.SetActive(true);
        _6_Down.SetActive(true);
        _6_Arrow.SetActive(true);
        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(9);
        AudioManager.Instance.PlayOneShot(correctSound);
    }

    public void PassPants_3_() // เมื่อดึงกางเกง/กระโปรงขึ้นแล้ว
    {
        if (ModuleManager.Instance.characterGender)
        {
            boyPushingPants.sprite = boy[9];
            bgKid.sprite = boy[11];
        }
        else
        {
            boyPushingPants.sprite = girl[9];
            bgKid.sprite = girl[11];
        }
        _6_Up.SetActive(false);
        _6_Down.SetActive(false);
        _6_Arrow.SetActive(false);
        poongPing_Pants.SetActive(true);
        AudioManager.Instance.PlayOneShot(correctSound);
        StartCoroutine(PassPants());
    }

    IEnumerator PassPants()
    {
        DisplayNextDialogue();
        if (!hasCompleted)
            PlayVO(10);
        CounterManager.Instance.SetEnableCounter(false);

        yield return new WaitForSeconds(VoiceOver[10].length);

        DisplayNextDialogue();
        PlayVO(11);
        shirt.SetActive(false);
        pants.SetActive(false);
        LayerBackground[1].transform.GetChild(2).gameObject.SetActive(false);
        poongPing_Pants.SetActive(true);

        mainArrow_1.SetActive(false);
        mainArrow_2.SetActive(false);


        LayerBackground[1].gameObject.gameObject.SetActive(true);
        LayerInteractable[1].gameObject.SetActive(true);

        LayerBackground[3].gameObject.gameObject.SetActive(false);
        LayerInteractable[3].gameObject.gameObject.SetActive(false);

        LayerBackground[1].transform.GetChild(3).gameObject.SetActive(true);
        LayerBackground[1].transform.GetChild(4).gameObject.SetActive(true);
        LayerBackground[1].transform.GetChild(5).gameObject.SetActive(false); // thinking shirt
        LayerBackground[1].transform.GetChild(6).gameObject.SetActive(false); // thinking pants

        yield return new WaitForSeconds(3f);

        DisplayNextDialogue();

        if (!hasCompleted)
            SetCompleteWithResult(1);
        else
        {
            if (mainArrow_1.GetComponent<Image>().enabled)
                SetCompleteWithResult(2);
            else
                SetCompleteWithResult(3);
        }
    }

    public override void PlayingModule()
    {
        if (!firstVO)
        {
            firstVO = true;
            StartCoroutine(PlayFirstVO());
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (shirt.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เมื่อคลิกลงบนพื้นที่รูปเสื้อ
            {
                deltaClick = Input.mousePosition - shirt.transform.position;
                holding = true;
                item = "shirt"; // ระบุว่ากำลังลากเสื้อ
            }
            if (pants.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เมื่อคลิกลงบนพื้นที่รูปกางเกง/กระโปรง
            {
                deltaClick = Input.mousePosition - pants.transform.position;
                holding = true;
                item = "pants"; // ระบุว่ากำลังลากกางเกง/กระโปรง
            }

            if (_1_Up.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เมื่อคลิกบน collider เริ่มต้นจะใส่แขนฝั่งซ้าย
            {
                handPushing_Tshirt.transform.position = Input.mousePosition;
                handPushing_Tshirt.SetActive(true);
                holding = true;
                item = "_1_";
            }
            if (_2_Up.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เมื่อคลิกบน collider เริ่มต้นจะใส่แขนฝั่งขวา
            {
                handPushing_Tshirt.transform.position = Input.mousePosition;
                handPushing_Tshirt.SetActive(true);
                holding = true;
                item = "_2_";
            }
            if (_3_Up.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เมื่อคลิกบน collider เริ่มต้นจะดึงเสื้อขึ้น
            {
                handPushing_Tshirt.transform.position = Input.mousePosition;
                handPushing_Tshirt.SetActive(true);
                holding = true;
                item = "_3_";
            }
            if (_4_Up.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เมื่อคลิกบน collider เริ่มต้นจะใส่ขาฝั่งซ้าย
            {
                handPushing_Pants.transform.position = Input.mousePosition;
                handPushing_Pants.SetActive(true);
                holding = true;
                item = "_4_";
            }
            if (_5_Up.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เมื่อคลิกบน collider เริ่มต้นจะใส่ขาฝั่งขวา
            {
                handPushing_Pants.transform.position = Input.mousePosition;
                handPushing_Pants.SetActive(true);
                holding = true;
                item = "_5_";
            }
            if (_6_Down.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // เมื่อคลิกบน collider เริ่มต้นจะดึงกางเกง/กระโปรงขึ้น
            {
                handPushing_Pants.transform.position = Input.mousePosition;
                handPushing_Pants.SetActive(true);
                holding = true;
                item = "_6_";
            }
        }
        else if (Input.GetMouseButton(0) && holding)
        {
            if (item == "shirt") // คลิกค้างที่เสื้อ ลากเสื้อตามนิ้ว
            {
                shirt.transform.position = Input.mousePosition - deltaClick;
            }
            else if (item == "pants" && !shirt.activeSelf) // คลิกค้างที่กางเกง ลากกางเกงตามนิ้ว (ต้องผ่านการใส่เสื้อก่อน)
            {
                pants.transform.position = Input.mousePosition - deltaClick;
            }
            else if (item == "_1_") // คลิกค้างเกมที่ 1
            {
                handPushing_Tshirt.transform.position = Input.mousePosition; // มีรูปมือตามนิ้ว
                if (_1_Down.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // ถ้าลากถึงพื้นที่จบ จะถือว่าผ่าน
                {
                    _1_pass = true;
                }
                else
                {
                    _1_pass = false;
                }
            }
            else if (item == "_2_") // คลิกค้างเกมที่ 2
            {
                handPushing_Tshirt.transform.position = Input.mousePosition; // มีรูปมือตามนิ้ว
                if (_2_Down.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // ถ้าลากถึงพื้นที่จบ จะถือว่าผ่าน
                {
                    _2_pass = true;
                }
                else
                {
                    _2_pass = false;
                }
            }
            else if (item == "_3_") // คลิกค้างเกมที่ 3
            {
                handPushing_Tshirt.transform.position = Input.mousePosition; // มีรูปมือตามนิ้ว
                if (_3_Down.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // ถ้าลากถึงพื้นที่จบ จะถือว่าผ่าน
                {
                    _3_pass = true;
                }
                else
                {
                    _3_pass = false;
                }
            }
            else if (item == "_4_") // คลิกค้างเกมที่ 4
            {
                handPushing_Pants.transform.position = Input.mousePosition; // มีรูปมือตามนิ้ว
                if (_4_Down.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // ถ้าลากถึงพื้นที่จบ จะถือว่าผ่าน
                {
                    _4_pass = true;
                }
                else
                {
                    _4_pass = false;
                }
            }
            else if (item == "_5_") // คลิกค้างเกมที่ 5
            {
                handPushing_Pants.transform.position = Input.mousePosition; // มีรูปมือตามนิ้ว
                if (_5_Down.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // ถ้าลากถึงพื้นที่จบ จะถือว่าผ่าน
                {
                    _5_pass = true;
                }
                else
                {
                    _5_pass = false;
                }
            }
            else if (item == "_6_") // คลิกค้างเกมที่ 6
            {
                handPushing_Pants.transform.position = Input.mousePosition; // มีรูปมือตามนิ้ว
                if (_6_Up.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Input.mousePosition)) // ถ้าลากถึงพื้นที่จบ จะถือว่าผ่าน
                {
                    _6_pass = true;
                }
                else
                {
                    _6_pass = false;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (tShirtTrigger) // ปล่อยเมาส์เมื่อลากเสื้อมาใส่ถูกจุด
            {
                PushTShirt();
            }
            if (_1_pass) // ปล่อยเมาส์เมื่อใส่แขนฝั่งซ้ายถูก
            {
                _1_pass = false;
                PassTshirt_1_();
            }
            if (_2_pass) // ปล่อยเมาส์เมื่อใส่แขนฝั่งขวาถูก
            {
                _2_pass = false;
                PassTshirt_2_();
            }
            if (_3_pass) // ปล่อยเมาส์เมื่อดึงเสื้อถูก
            {
                _3_pass = false;
                PassTshirt_3_();
            }
            if (pantsTrigger) // ปล่อยเมาส์เมื่อลากกางเกง / กระโปรงมาใส่ถูกจุด
            {
                PushPants();
            }
            if (_4_pass) // ปล่อยเมาส์เมื่อใส่ขาฝั่งซ้ายถูก
            {
                _4_pass = false;
                PassPants_1_();
            }
            if (_5_pass) // ปล่อยเมาส์เมื่อใส่ขาฝั่งขวาถูก
            {
                _5_pass = false;
                PassPants_2_();
            }
            if (_6_pass) //ปล่อยเมาส์เมื่อดึงกางเกง / กระโปรงถูก
            {
                _6_pass = false;
                PassPants_3_();
            }
            if (!item.Equals("empty")) // Reset item to empty and reset position to initial
            {
                shirt.transform.localPosition = initPosShirt;
                pants.transform.localPosition = initPosPants;
                item = "empty";
                handPushing_Tshirt.SetActive(false);
                handPushing_Pants.SetActive(false);
            }
            holding = false;
        }
        else
        {
            if(LayerBackground[1].gameObject.activeSelf) // ถ้ายังลากเสื้อ/กางเกง/กระโปรง ไม่ถูก ให้ reset ตำแหน่งรูปเสื้อ/กางเกง/กระโปรง
            {
                pants.transform.localPosition = initPosPants;
                shirt.transform.localPosition = initPosShirt;
            }
        }
    }

    IEnumerator PlayFirstVO()
    {
        if (!hasCompleted)
        {
            PlayVO(0);
            yield return new WaitForSeconds(VoiceOver[0].length);
            LayerInteractable[0].transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true; // มือชี้ตู้
        }
        else
        {
            PlayVO(12);
            DialogueManager.Instance.ForceSetText("เด็ก ๆ ต้องทำอะไรนะ");
            yield return new WaitForSeconds(3f);
            DialogueManager.Instance.ForceSetText("ใช่แล้ว ไปแต่งตัวกัน");
            yield return new WaitForSeconds(VoiceOver[12].length - 3f);
        }
        LayerInteractable[0].transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true; // door
        CounterManager.Instance.SetEnableCounter(true);
    }
}
