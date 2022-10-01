using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandleEntries : MonoBehaviour {
    private int _numEntries;
    private float offset = 0.05f*Screen.height;
    public List<RectTransform> _entries;
    public GameObject entry;
    public List<RectTransform> fieldsBelow;
    public HandleEntries handler;

    public void Start() {
        _numEntries = 1;
        if (handler.gameObject)
            handler._entries.Add(gameObject.GetComponent<RectTransform>());
    }
    
    // used by the top level add button
    public void CreateEntry() {
        var newEntry = Instantiate(entry, transform.parent, false);
        newEntry.transform.GetChild(2).GetComponent<HandleEntries>().handler = this;
        
        var rTrans = newEntry.GetComponent<RectTransform>();
        var anchoredPos = rTrans.anchoredPosition;
        anchoredPos = new Vector2(anchoredPos.x,anchoredPos.y - _numEntries*offset);
        rTrans.anchoredPosition = anchoredPos;

        foreach (var field in fieldsBelow) {
            anchoredPos = field.anchoredPosition;
            anchoredPos = new Vector2(anchoredPos.x,anchoredPos.y - offset);
            field.anchoredPosition = anchoredPos;
        }
        _numEntries++;
    }

    // used by the entries themselves (in the delete button)
    public void DeleteEntry() {
        var myTrans = gameObject.GetComponent<RectTransform>();
        var below = false;
        handler._numEntries--;
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
