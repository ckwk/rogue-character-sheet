using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceSwiper : MonoBehaviour, IDragHandler, IEndDragHandler {
    private Text numDice, typeDice;
    private GameObject currentDice, lastDice, total, seperator;
    private RectTransform _transform;
    private Vector2 startPosition, startSize;
    private Rect startRect;
    private Toggle automatic, on3, on2, on1, infinitely, twice, thirdHigest, secondHighest;
    private List<GameObject> explodedDice;
    private int totalRoll;
    private GameManager _gm;
    public Sprite d4Gold, d6Gold, d7Gold, d8Gold, d10Gold;
    
    public void Start() {
        numDice = GameObject.Find("Number").transform.GetChild(0).GetComponent<Text>();
        typeDice = GameObject.Find("TypeDice").transform.GetChild(0).GetComponent<Text>();
        total = GameObject.Find("Total");
        total.SetActive(false);
        seperator = GameObject.Find("Seperator");
        seperator.SetActive(false);
        _transform = gameObject.GetComponent<RectTransform>();
        startPosition = _transform.anchoredPosition;
        startSize = _transform.sizeDelta;
        startRect = _transform.rect;
        automatic = GameObject.Find("ToggleAutomatic").GetComponent<Toggle>();
        on3 = GameObject.Find("ToggleOn3").GetComponent<Toggle>();
        on2 = GameObject.Find("ToggleOn2").GetComponent<Toggle>();
        on1 = GameObject.Find("ToggleOn1").GetComponent<Toggle>();
        infinitely = GameObject.Find("ToggleInfinitely").GetComponent<Toggle>();
        twice = GameObject.Find("ToggleTwice").GetComponent<Toggle>();
        thirdHigest = GameObject.Find("ToggleThirdHighest").GetComponent<Toggle>();
        secondHighest = GameObject.Find("ToggleSecondHighest").GetComponent<Toggle>();
        explodedDice = new List<GameObject>();
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnDrag(PointerEventData eventData) {
        var distance = eventData.position.x - eventData.pressPosition.x;
        _transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, startRect.width + distance);
        _transform.anchoredPosition = new Vector2(startPosition.x + distance/2, _transform.anchoredPosition.y);
    }

    public void OnEndDrag(PointerEventData eventData) {
        var distance = eventData.position.x - eventData.pressPosition.x;
        if (distance > 0.5 * Screen.width) {
            Debug.Log("Rolling dice...");
            _gm.PlayRollSound();
            RollDice();
        }
        StartCoroutine(ReturnToStartSize());
    }

    private void RollDice() {
        if (int.Parse(numDice.text) <= 10) currentDice = GameObject.Find("DiceMenu").transform.GetChild(int.Parse(numDice.text)).Find("d" + typeDice.text).gameObject;
        else currentDice = GameObject.Find("DiceMenu").transform.GetChild(11).Find("d" + typeDice.text).gameObject;
        total.SetActive(true);
        seperator.SetActive(true);
        totalRoll = 0;
        if (lastDice != null) lastDice.SetActive(false);
        currentDice.SetActive(true);
        lastDice = currentDice;

        foreach (var die in explodedDice) {
            die.GetComponent<Image>().overrideSprite = null;
        }
        explodedDice.Clear();
        
        foreach (Transform child in currentDice.transform) {
            total.GetComponent<Text>().text = "0";
            total.SetActive(false);
            if (child.name != "...") StartCoroutine(RollDice(child.gameObject));
            else StartCoroutine(RollMiscDice(int.Parse(numDice.text)));
        }
    }
    
    IEnumerator ReturnToStartSize() {
        var t = 0f;
        var stretchedPos = _transform.anchoredPosition;
        var stretchedSize = _transform.sizeDelta;
        while (t < 1) {
            t += Time.deltaTime / 0.5f;
            _transform.anchoredPosition = Vector2.Lerp(stretchedPos, startPosition, Mathf.SmoothStep(0f,1f, t));
            _transform.sizeDelta = Vector2.Lerp(stretchedSize, startSize, Mathf.SmoothStep(0f,1f, t));
            yield return null;
        }
    }

    IEnumerator RollDice(GameObject die) {
        var t = 0f;
        var randomGenerationTimer = 0f;
        var newRandomBoundary = 0.1f;
        var lastNum = 0;
        while (t < 1) {
            t += Time.deltaTime / (1 + 0.1f * (int.Parse(die.name) - 1));
            randomGenerationTimer += Time.deltaTime;
            if (randomGenerationTimer > newRandomBoundary) {
                randomGenerationTimer = 0f;
                var num = Random.Range(1, int.Parse(typeDice.text)+1);
                if (num == lastNum) {
                    if (num == 1) num++;
                    else num--;
                }
                die.transform.GetChild(0).GetComponent<Text>().text = num.ToString();
                lastNum = num;
            }
            yield return null;
        }

        total.SetActive(true);
        var dieVal = int.Parse(die.transform.GetChild(0).GetComponent<Text>().text);
        var canExplode = dieVal == int.Parse(typeDice.text) || automatic.isOn || on3.isOn && dieVal < 4 || 
                         on2.isOn && dieVal < 3 || on1.isOn && dieVal == 1 || 
                         thirdHigest.isOn && dieVal >= int.Parse(typeDice.text) - 2 ||
                         secondHighest.isOn && dieVal >= int.Parse(typeDice.text) - 1;

        if (canExplode) {
            var explodeTotal = 0;
            var explodeDie = Random.Range(1, int.Parse(typeDice.text) + 1);
            explodeTotal += explodeDie;
            if (twice.isOn || infinitely.isOn) {
                do {
                    canExplode = explodeDie == int.Parse(typeDice.text) || on3.isOn && explodeDie < 4 || 
                                 on2.isOn && explodeDie < 3 || on1.isOn && explodeDie == 1 || 
                                 thirdHigest.isOn && explodeDie >= int.Parse(typeDice.text) - 2 ||
                                 secondHighest.isOn && explodeDie >= int.Parse(typeDice.text) - 1;
                    if (!canExplode) continue;
                    explodeDie = Random.Range(1, int.Parse(typeDice.text) + 1);
                    explodeTotal += explodeDie;
                    if (explodeTotal <= 10000) continue;
                    explodeTotal = 10000;
                    break;
                } while (canExplode && infinitely.isOn);
            }
            
            dieVal += explodeTotal;
            explodedDice.Add(die);
            if (dieVal < 10000) die.transform.GetChild(0).GetComponent<Text>().text = dieVal.ToString();
            else die.transform.GetChild(0).GetComponent<Text>().text = "∞";
            die.GetComponent<Image>().overrideSprite = die.transform.parent.name switch {
                "d4" => d4Gold,
                "d6" => d6Gold,
                "d7" => d7Gold,
                "d8" => d8Gold,
                "d10" => d10Gold,
                _ => die.GetComponent<Image>().overrideSprite
            };
        }
        totalRoll += dieVal;
        total.GetComponent<Text>().text = totalRoll < 10000 ? totalRoll.ToString() : "∞";
    }

    IEnumerator RollMiscDice(int num) {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < num; i++) {
            totalRoll += Random.Range(1, int.Parse(typeDice.text) + 1);
            total.GetComponent<Text>().text = totalRoll < 10000 ? totalRoll.ToString() : "∞";
            yield return new WaitForSeconds(0.1f);
        }
    }
}
