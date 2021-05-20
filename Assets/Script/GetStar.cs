using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetStar : MonoBehaviour
{
    public List<Image> Stars = new List<Image>();
    public List<Image> FullStars = new List<Image>();
    //public List<Sprite> FullStars = new List<Sprite>();
    //public List<Sprite> NonStars = new List<Sprite>();

    public Button NextButton;
    public Sprite NextButtonLockSprite;
    public Sprite NextButtonUnlockSprite;

    public Button ReplayButton;

    public AudioClip get3star;
    public AudioClip get1or2star;
    public void SetStar(int star)
    {
        //Debug.Log("Get " + star + " stars");
        ReplayButton.interactable = true;
        StartCoroutine(ShowStar(star));

    }

    public void SetNextButtonEnable()
    {
        NextButton.image.sprite = NextButtonUnlockSprite;
        NextButton.interactable = true;
    }

    public void ResetNextButtonEnable()
    {

        NextButton.image.sprite = NextButtonLockSprite;
        NextButton.interactable = false;
        FullStars[0].gameObject.SetActive(false);
        FullStars[1].gameObject.SetActive(false);
        FullStars[2].gameObject.SetActive(false);
    }

    IEnumerator ShowStar(int star)
    {
        for (int i = 0; i < Stars.Count; i++)
        {
            if (star > i)
            {
                yield return new WaitForSeconds(0.3f);
                FullStars[i].gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.5f);
        if (star == 3)
            AudioManager.Instance.PlayVoiceOver(get3star, false);
        else
            AudioManager.Instance.PlayVoiceOver(get1or2star, false);
    }

    public void ButtonClickSound(AudioClip clip)
    {
        AudioManager.Instance.PlayOneShot(clip);
    }
}
