using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimationModule : Module
{
    public bool useAnimatorTime = false;
    public Animator animator;
    public bool playAnimation = false;
    public Button nextButton;

    public GameObject kid;
    public Sprite boySleep;
    public Sprite girlSleep;

    public Sprite bedStart;
    public GameObject kidDance;
    public override void ResetModule()
    {
        ProgressBarManager.Instance.SetEnableProgressBar(false);

        if (kidDance)
            kidDance.SetActive(false);

        if (LayerBackground[0].transform.GetChild(4).gameObject) // sleeping kid
            LayerBackground[0].transform.GetChild(4).gameObject.SetActive(true);

        if (LayerBackground[0].transform.GetChild(5).gameObject) // blanket
            LayerBackground[0].transform.GetChild(5).gameObject.SetActive(true);

        if (kid)
        {
            if (ModuleManager.Instance.characterGender)
                kid.GetComponent<Image>().sprite = boySleep;
            else
                kid.GetComponent<Image>().sprite = girlSleep;
        }

        playAnimation = false;
        complete = false;

        if (nextButton)
            nextButton.interactable = true;
    }

    public override void PlayingModule()
    {
        if (!playAnimation)
        {
            if (ModuleManager.Instance.characterGender)
                animator.SetBool("Gender", true);
            else
                animator.SetBool("Gender", false);

            playAnimation = true;

            StartCoroutine(WaitForAnimator());
        }

        if (complete)
        {
            complete = false;
            StartCoroutine(ModuleManager.Instance.FadeChangeModule());

        }
    }

    IEnumerator WaitForAnimator()
    {
        if (useAnimatorTime)
            TriggerDialogue();
        else
            StartCoroutine(DialogueForWakeUp());

        if (nextButton)
            nextButton.gameObject.SetActive(false);

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float totaltime = 0f;
        foreach (AnimationClip clip in clips)
        {
            totaltime = totaltime + clip.length;
        }
        yield return new WaitForSeconds(totaltime / 1.2f);

        if (useAnimatorTime)
            SetComplete();
        else
        {
            nextButton.gameObject.SetActive(true);
            DisplayNextDialogue();
        }
    }
    IEnumerator DialogueForWakeUp()
    {
        yield return new WaitForSeconds(1f);
        TriggerDialogue();
        yield return new WaitForSeconds(3f);
        DisplayNextDialogue();
        AudioManager.Instance.PlayVoiceOver(VoiceOver[0],checkVO[0]);
    }
}
