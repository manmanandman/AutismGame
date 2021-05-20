using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpongeController : MonoBehaviour
{
    public WashDishModule washDishModule;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("collider_0"))
        {
            washDishModule.rHand.GetComponent<Image>().sprite = washDishModule.rHand_s[1];
            washDishModule.liquidOvering = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("collider_0"))
        {
            washDishModule.rHand.GetComponent<Image>().sprite = washDishModule.rHand_s[0];
            washDishModule.liquidOvering = false;
        }
    }
}
