using System.Collections;
using UnityEngine;

public class SaveBanner : MonoBehaviour
{
    private GameManager _gm;
    private Vector2 _startPos,
        _loweredPos;
    private bool alreadyActive;
    private float _speed = 10f,
        offset = Screen.height * 0.045f;

    private void Awake()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _startPos = transform.position;
        _loweredPos = _startPos + Vector2.down * offset;
    }

    public void Appear()
    {
        if (alreadyActive)
            return;
        gameObject.SetActive(true);
        StartCoroutine(Slide(true));
        alreadyActive = true;
    }

    public void Disappear()
    {
        StartCoroutine(Slide(false));
        alreadyActive = false;
    }

    public IEnumerator Slide(bool down)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * _speed;
            if (down)
                transform.position = Vector2.Lerp(
                    _startPos,
                    _loweredPos,
                    Mathf.SmoothStep(0f, 1f, t)
                );
            else
                transform.position = Vector2.Lerp(
                    _loweredPos,
                    _startPos,
                    Mathf.SmoothStep(0f, 1f, t)
                );
            yield return null;
        }
    }

    public void Save()
    {
        _gm.SaveCharacter();
        //StartCoroutine(Slide(false));
        alreadyActive = false;
    }
}
