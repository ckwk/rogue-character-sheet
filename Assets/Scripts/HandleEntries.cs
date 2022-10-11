using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class HandleEntries : MonoBehaviour {
    private int _numEntries;
    private float offset = 0.05f*Screen.height;
    public List<RectTransform> _entries;
    public GameObject entry;
    public List<RectTransform> fieldsBelow;
    public HandleEntries handler;
    public VerticalScroll scroller;

    public void Start() {
        _numEntries = 1;
        if (!handler.gameObject) return;
        
        handler._entries.Add(gameObject.GetComponent<RectTransform>());
        if (handler.fieldsBelow.Count < 1) scroller.lowestEntry = gameObject.GetComponent<RectTransform>();
    }
    
    // used by the top level add button
    public void CreateEntry() {
        var newEntry = Instantiate(entry, transform.parent, false);
        newEntry.transform.GetChild(1).GetComponent<HandleEntries>().handler = this;
        newEntry.transform.GetChild(1).GetComponent<HandleEntries>().scroller = scroller;

        var rTrans = newEntry.GetComponent<RectTransform>();
        var anchoredPos = rTrans.anchoredPosition;
        anchoredPos = new Vector2(anchoredPos.x,anchoredPos.y - _numEntries*offset);
        rTrans.anchoredPosition = anchoredPos;

        foreach (var field in fieldsBelow) {
            anchoredPos = field.anchoredPosition;
            anchoredPos = new Vector2(anchoredPos.x,anchoredPos.y - offset);
            field.anchoredPosition = anchoredPos;
        }

        scroller.numEntries++;
        _numEntries++;
    }

    // used by the entries themselves (in the delete button)
    public void DeleteEntry() {
        var myTrans = gameObject.GetComponent<RectTransform>();
        var below = false;
        handler._numEntries--;
        scroller.numEntries--;
        foreach (var e in handler._entries) {
            if (e == myTrans) {
                below = true;
            } else if (!below) {
                continue;
            }

            var anchoredPos = e.parent.GetComponent<RectTransform>().anchoredPosition;
            e.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredPos.x,anchoredPos.y + offset);
        }
        
        foreach (var field in handler.fieldsBelow) {
            var anchoredPos = field.anchoredPosition;
            anchoredPos = new Vector2(anchoredPos.x,anchoredPos.y + offset);
            field.anchoredPosition = anchoredPos;
        }
        
        handler._entries.Remove(myTrans);
        Destroy(transform.parent.gameObject);
    }
}
