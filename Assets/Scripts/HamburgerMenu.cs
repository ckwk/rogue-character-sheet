using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HamburgerMenu : MonoBehaviour {
    private Dropdown dd;
    private GameManager _gm;
    public GameObject loadCharacterMenu, loadButton;
    public Transform LoadDropdownContent;
    private void Start() {
        dd = GetComponent<Dropdown>();
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Hamburger() {
        switch (dd.value) {
            case 0: break;
            case 1: _gm.SaveCharacter(); break;
            case 2: LoadCharacters(); break;
            case 3: CloneCharacter(); break;
            case 4: CreateNewCharacter(); break;
            default: break;
        }
        
        _gm.ResetHamburger();
    }

    private void CreateNewCharacter() {
        var directory = new DirectoryInfo(Application.persistentDataPath + "/New Character​");
        var path = Application.persistentDataPath + "/New Character​";
        path += "/" + directory.GetFiles().OrderBy(f => f.CreationTime).First().Name;
        _gm.LoadCharacter(path);
        PlayerPrefs.SetString("lastFile", path);
    }

    private void CloneCharacter() {
        _gm.currentCharacter.name = _gm.currentCharacter.name + " (Clone)";
        _gm.SetUIName();
        _gm.SaveCharacter();
    }

    private void LoadCharacters() {
        var directories = Directory.GetDirectories(Application.persistentDataPath).ToList();
        var numButtons = 0;
        
        loadCharacterMenu.SetActive(true);
        foreach (var charName in directories.Select(directory => directory.Replace('\\', '/'))
            .Select(charName => charName.Substring(charName.LastIndexOf('/') + 1))) {
            if (charName == "New Character​" || charName == "il2cpp") continue;
            var button = Instantiate(loadButton,LoadDropdownContent);
            button.GetComponent<RectTransform>().anchoredPosition += Vector2.down * (numButtons * 146);
            button.transform.GetChild(0).GetComponent<Text>().text = charName;
            numButtons++;

        }
    }
}
