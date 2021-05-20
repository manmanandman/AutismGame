using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsController : MonoBehaviour
{
    public DressingModule dressingModule;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("BottomCollider"))
        {
            dressingModule.pantsTrigger = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("BottomCollider"))
        {
            dressingModule.pantsTrigger = false;
        }
    }
}
