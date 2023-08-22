using UnityEngine;
using UnityEngine.UI;

public class SpellStatButton : MonoBehaviour
{
    private Text stat;
    private GameManager gm;
    private string[] mutationStats = { "STR", "DEX", "END", "INT", "DEX" };
    private int mutationIndex = 0;

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
        mutationIndex = mutationIndex < mutationStats.Length ? 0 : mutationIndex + 1;
        stat.text = mutationStats[mutationIndex];
        gm.SetCharacterMutationStat();
    }
}
