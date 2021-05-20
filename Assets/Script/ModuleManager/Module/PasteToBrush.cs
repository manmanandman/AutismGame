using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasteToBrush : MonoBehaviour
{
    public BrushTeethModule brushTeeth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("check (0)") && !brushTeeth.firstTrig) // ถ้าหัวหลอดยาสีฟันชนขนแปรงจุดแรก (โดยที่ไม่เคยชนมาก่อน)
        {
            brushTeeth.FirstTriger();
        }
        else if (collision.gameObject.name.Equals("check (1)") && brushTeeth.firstTrig) // ถ้าหัวหลอดยาสีฟันชนขนแปรงจุดที่สอง โดยที่ชนจุดแรกมาก่อนแล้ว
        {
            brushTeeth.SecondTrigger();
        }
    }
}
