using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterModule : Module
{
    private Button boyButton;
    private Button girlButton;
    public override void ResetModule() {
        ProgressBarManager.Instance.SetEnableProgressBar(false);
        LayerBackground[0].gameObject.SetActive(true);
        LayerInteractable[0].gameObject.SetActive(true);
        boyButton = LayerInteractable[0].transform.GetChild(0).transform.GetChild(0).GetComponent<Button>();
        girlButton = LayerInteractable[0].transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        boyButton.interactable = true;
        girlButton.interactable = true;
    }

    public void ClickButton(bool selectCharacter) // true = boy , false = girl
    {
        ModuleManager.Instance.characterGender = selectCharacter;
        boyButton.interactable = false;
        girlButton.interactable = false;
        SetComplete();
        StartCoroutine(ModuleManager.Instance.FadeChangeModule());
    }

}


