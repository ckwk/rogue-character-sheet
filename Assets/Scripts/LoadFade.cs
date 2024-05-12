using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFade : MonoBehaviour
{
    public GameObject diceSwipeBar,
        swipeToRoll,
        diceButton;

    public void Disappear()
    {
        foreach (Transform button in transform.parent.GetChild(1).GetChild(0).GetChild(0))
        {
            Destroy(button.gameObject);
        }

        transform.parent.gameObject.SetActive(false);
    }

    public void NormalDisappear()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void DiceFadeDisappear()
    {
        transform.parent.gameObject.SetActive(false);
        diceSwipeBar.SetActive(false);
        swipeToRoll.SetActive(false);
        diceButton.SetActive(true);
    }
}
