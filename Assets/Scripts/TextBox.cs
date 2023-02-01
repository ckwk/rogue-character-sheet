using static GameManager;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    private Text myText;
    private InputField inField;

    private void Start()
    {
        myText = gameObject.GetComponent<Text>();
        inField = GameObject.Find("GameManager").GetComponent<GameManager>().inField;
    }

    private void Update()
    {
        myText.text = inField.text;
    }
}
