using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushUpDown : MonoBehaviour
{
    public float percentPerHit; // ค่าที่จะเพิ่มขึ้นเมื่อแปรงผ่าน collider ฟันบน/ล่าง เต็ม 1 (เอาไว้ปรับค่าใน Unity)
    public BrushTeethModule brushTeeth; // Module หลัก
    public Image bubble;
    public Image food;
    public Image brush;
    public List<Sprite> brushFlip;
    public GameObject firstArrow;
    public GameObject upArrow;
    public GameObject downArrow;
    public bool firstCome; //แปรงชนฟันครั้งแรก (ฟันล่าง)
    public float half;
    public float halfquater;

    private bool upped = false; //toggle check บน/ล่าง

    private bool dialogueA = false;
    private bool dialogueB = false;
    private bool dialogueC = false;
    private void Start()
    {
        brush.sprite = brushFlip[0];
        firstArrow.SetActive(true);
        upArrow.SetActive(false);
        downArrow.SetActive(false);
        firstCome = false;

    }

    private void Update()
    {
        if (bubble.color.a >= 1f) // ถ้ารูปฟองแสดงชัดแล้ว (a max at 1)
        {
            upArrow.SetActive(false);
            downArrow.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            brush.sprite = brushFlip[0];
            if (bubble.color.a >= 1f) // ถ้ารูปฟองแสดงชัดแล้ว (a max at 1)
            {
                bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, 0f); // reset ให้ฟองหาย
                food.color = new Color(food.color.r, food.color.g, food.color.b, 1f); // reset ให้เห็นเศษอาหาร
                brushTeeth.initPos = new Vector3(0, 0, 0);
                brushTeeth.completeBrushing = true;
                brushTeeth.BrushTeethGame1.SetActive(false);
                brushTeeth.DisplayNextDialogue();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("LowTooth"))
        {
            if (upped) // ถ้าชนฟันล่าง โดยที่ชนฟันบนมาก่อนแล้ว
            {
                upped = false;
                bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, bubble.color.a + percentPerHit); // รูปฟองชัดขึ้นตาม percentPerHit
                food.color = new Color(food.color.r, food.color.g, food.color.b, food.color.a - percentPerHit);  // รูปเศษอาหารจางลงตาม percentPerHit
                upArrow.SetActive(true);
                downArrow.SetActive(false);
            }
            if (!firstCome) // ถ้ามาชนครั้งแรกในเกม
            {
                brush.sprite = brushFlip[1];
                firstArrow.SetActive(false);
                upArrow.SetActive(true);
                firstCome = true;
                upped = false;
            }
        }

        if (collision.gameObject.name.Equals("UpTooth") && firstCome && !upped) // ถ้าชนฟันบน โดยที่ชนฟันล่างมาก่อนแล้ว
        {
            upped = true;
            bubble.color = new Color(bubble.color.r, bubble.color.g, bubble.color.b, bubble.color.a + percentPerHit); // รูปฟองชัดขึ้นตาม percentPerHit
            food.color = new Color(food.color.r, food.color.g, food.color.b, food.color.a - percentPerHit); // รูปเศษอาหารจางลงตาม percentPerHit
            upArrow.SetActive(false);
            downArrow.SetActive(true);
        }
        if (bubble.color.a == half && !dialogueA)
        {
            brushTeeth.DisplayNextDialogue();
            dialogueA = true;
        }
        if (bubble.color.a == halfquater & !dialogueB)
        {
            brushTeeth.DisplayNextDialogue();
            dialogueB = true;
        }
        if (bubble.color.a >= 1 & !dialogueC)
        {
            brushTeeth.DisplayNextDialogue();
            dialogueC = true;
        }
    }

    public void resetDialogue()
    {
        dialogueA = false;
        dialogueB = false;
        dialogueC = false;
    }
}
