using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TshirtController : MonoBehaviour
{
    public DressingModule dressingModule; // Module หลัก

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("TopCollider")) // ชนพื้นที่ตัวด้านบน
        {
            dressingModule.tShirtTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // ไม่ชนพื้นที่ตัวด้านบน
    {
        if (collision.gameObject.name.Equals("TopCollider"))
        {
            dressingModule.tShirtTrigger = false;
        }
    }
}
