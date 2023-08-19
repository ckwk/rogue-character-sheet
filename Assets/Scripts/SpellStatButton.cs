using UnityEngine;
using UnityEngine.UI;

public class SpellStatButton : MonoBehaviour
{
    private Text stat;

    private void Start()
    {
        stat = transform.GetChild(0).GetComponent<Text>();
    }

    public void SwitchStat()
    {
        stat.text = stat.text == "INT" ? "CHA" : "INT";
    }
}
