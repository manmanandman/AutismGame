using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModuleGetStar : Module
{

    public override void PlayingModule()
    {
        if (complete)
        {
            SetCompleteWithResult(3); //input star here
            //ModuleManager.Instance.LayerResult.SetActive(true);
            complete = false;
        }
    }
}
