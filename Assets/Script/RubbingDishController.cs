using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbingDishController : MonoBehaviour
{
    public WashDishModule washDishModule;
    public int hitNumberForPass;

    private int hit = 0; 
    private string triggerName;

    private void Start()
    {
        hit = 0;
    }

    private void Update()
    {
        if (hit >= hitNumberForPass)
        {
            washDishModule.passRubbingDish = true;
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("collider_dish_"))
        {
            if (!collision.name.Equals(triggerName))
            {
                triggerName = collision.name;
                hit++;
            }
        }    
    }
}
