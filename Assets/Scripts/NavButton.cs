using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavButton : MonoBehaviour
{
    private GameManager _gm;
    public GameObject page;
    public Text myText;
    public Image myIcon;

    private void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void NavigateToPage()
    {
        var darkendColour = new Color(1, 1, 1, 0.6f);
        foreach (var text in _gm.navText)
        {
            text.color = darkendColour;
        }
        myText.color = Color.white;

        foreach (var icon in _gm.navIcons)
        {
            icon.color = darkendColour;
        }
        myIcon.color = Color.white;

        foreach (var p in _gm.pages)
        {
            p.SetActive(false);
        }
        page.SetActive(true);
    }
}
