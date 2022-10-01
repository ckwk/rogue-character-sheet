using System.Collections;
using UnityEngine;

public class SaveBanner : MonoBehaviour {
    private GameManager _gm;
    private RectTransform _trans;
    private Vector2 _startPos, _loweredPos;
    private float _speed = 0.7f;

    private void Start() {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _startPos = _trans.anchoredPosition;
        _loweredPos = _startPos + Vector2.down * _trans.sizeDelta.y;
    }

    public IEnumerator Slide(bool down) {
        var t = 0f;
        while (t < 1) {
            t += Time.deltaTime * _speed;
            if (down) _trans.anchoredPosition = Vector2.Lerp(_startPos, _loweredPos, Mathf.SmoothStep(0f, 1f, t));
            else _trans.anchoredPosition = Vector2.Lerp(_loweredPos, _startPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    public void Save() {
        _gm.SaveCharacter();
        StartCoroutine(Slide(false));
    }
}
