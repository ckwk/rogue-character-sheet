using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;

public class VerticalScroll : MonoBehaviour, IDragHandler, IEndDragHandler {
    private Vector3 panelLocation, startLocation;
    private int startingNumEntries;
    public int numEntries = 2, entriesUntilScroll = 14;
    private float distance = Screen.height, baseOffset = Screen.height*0.1f, entryOffset = Screen.height*0.05f;
    public float easing = 0.5f;
    public RectTransform lowestEntry;
    public Transform view;

    private void Start() {
        panelLocation = view.position;
        startLocation = panelLocation;
        startingNumEntries = numEntries;
    }

    public void OnDrag(PointerEventData eventData) {
        if (numEntries <= entriesUntilScroll) return;
        var difference = (eventData.pressPosition.y - eventData.position.y) / Screen.height * distance;
        view.position = panelLocation - new Vector3(0,difference);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (numEntries <= entriesUntilScroll) return;
        if (view.position.y < startLocation.y)
            StartCoroutine(SmoothMove(view.position, startLocation, easing));
        if (view.position.y > startLocation.y + entryOffset * (numEntries-6)) {
            var newLocation = new Vector3(view.position.x, startLocation.y + entryOffset * (numEntries-6));
            StartCoroutine(SmoothMove(view.position, newLocation, easing)); 
        }
            
        panelLocation = view.position;
    }

    public void ScrollUp() {
        StartCoroutine(SmoothMove(view.position, startLocation, easing));
    }
    
    IEnumerator SmoothMove(Vector3 start, Vector3 end, float seconds) {
        float t = 0f;
        while (t < 1) {
            t += Time.deltaTime / seconds;
            view.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0f,1f, t));
            yield return null;
        }

        panelLocation = view.position;
    }
}
