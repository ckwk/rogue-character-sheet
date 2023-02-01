using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation,
        viewStartLocation;
    private Transform viewTrans;
    private float distance = Screen.width;
    public float percentThreshold = 0.2f,
        easing = 0.5f;

    private void Start()
    {
        panelLocation = transform.position;
        viewTrans = transform.GetChild(0);
        viewStartLocation = viewTrans.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var difference =
            (eventData.pressPosition.x - eventData.position.x) / Screen.width * distance;
        transform.position = panelLocation - new Vector3(difference, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if (
            Mathf.Abs(percentage) > percentThreshold
            && (
                GameManager.currentScreen == 0
                || (int)Mathf.Sign(percentage) != GameManager.currentScreen
            )
        )
        {
            var newLocation = panelLocation;
            newLocation += new Vector3(distance * -Mathf.Sign(percentage), 0, 0);
            GameManager.currentScreen += newLocation.x > transform.position.x ? -1 : 1;
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    IEnumerator SmoothMove(Vector3 start, Vector3 end, float seconds)
    {
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
