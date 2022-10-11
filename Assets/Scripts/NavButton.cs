using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavButton : MonoBehaviour {
    private GameManager _gm;
    public GameObject page;

    private void Start() {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void NavigateToPage() {
        foreach (var p in _gm.pages) {
            p.SetActive(false);
        }
        page.SetActive(true);
    }
}
