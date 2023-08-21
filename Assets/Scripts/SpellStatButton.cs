using UnityEngine;
using UnityEngine.UI;

public class SpellStatButton : MonoBehaviour
{
    private Text stat;
    private GameManager gm;

    private void Start()
    {
        stat = transform.GetChild(0).GetComponent<Text>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SwitchStat()
    {
        stat.text = stat.text == "INT" ? "CHA" : "INT";
        gm.SetCharacterSpellStat();
    }

    public void SwitchMutationStat()
    {
        switch (stat.text)
        {
            case "INT":
                stat.text = "CHA";
                break;
            case "CHA":
                stat.text = "STR";
                break;
            case "STR":
                stat.text = "DEX";
                break;
            case "DEX":
                stat.text = "END";
                break;
            case "END":
                stat.text = "INT";
                break;
            default:
                stat.text = "INT";
                break;
        }
        gm.SetCharacterMutationStat();
    }
}
