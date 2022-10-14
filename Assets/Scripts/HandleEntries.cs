using System.Collections.Generic;
using UnityEngine;

public class HandleEntries : MonoBehaviour {
    private int _numEntries;
    private float offset = 0.05f*Screen.height;
    public int index = -1;
    public List<string> serializedEntries;
    public List<RectTransform> _entries;
    public GameObject entry;
    public List<RectTransform> fieldsBelow;
    public HandleEntries handler;
    public VerticalScroll scroller;

    private void Awake() {
        serializedEntries = new List<string> {"|||"};
    }

    public void Start() {
        serializedEntries = new List<string> {"|||"};
        _numEntries = 1;
        if (!handler) return;
        handler._entries.Add(gameObject.GetComponent<RectTransform>());
        if (handler.fieldsBelow.Count < 1) scroller.lowestEntry = gameObject.GetComponent<RectTransform>();
    }
    
    // used by the top level add button
    public void CreateEntry() {
        var newEntry = Instantiate(entry, transform.parent, false);
        var entryScript = newEntry.transform.GetChild(1).GetComponent<HandleEntries>();
        entryScript.handler = this;
        entryScript.scroller = scroller;
        entryScript.index = _numEntries;
        serializedEntries.Add("|||");

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
