using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static bool EveryOtherFrame, EveryFourFrames, EveryEightFrames, Every32Frames, EverySecond;
    public static bool UnsavedChanges;
    public static int currentScreen;
    private float count;
    private int frameCount;
    private GameObject diceMenu, diceSwipeBar, swipeToRoll, saveBanner;
    private AudioSource mainAudio;
    public AudioClip buttonPress, diceRoll;
    public Character currentCharacter;
    public TMP_Text name;
    public InputField inField, numDice;
    public TextBox activeTextBox;
    private Text diceType;
    public Text level,
        hitpoints,
        maxHitpoints,
        ac,
        strength,
        dexterity,
        endurance,
        intelligence,
        charisma,
        intuition,
        luck,
        strokesOfLuck,
        traits,
        bioAndNotes,
        equipment,
        weapons,
        armour,
        spells,
        cAndM;

    public Dropdown hamburger;
    public Image portrait;
    public Texture2D defaultPortrait;
    public TMP_InputField nameF;
    public InputField levelF,
        hitpointsF,
        maxHitpointsF,
        acF,
        strengthF,
        dexterityF,
        enduranceF,
        intelligenceF,
        charismaF,
        intuitionF,
        luckF;


    private void Awake() {
        currentCharacter = new Character();
        UnsavedChanges = false;
    }

    private void Start() {
        mainAudio = Camera.main.GetComponent<AudioSource>();
        numDice = GameObject.Find("Number").GetComponent<InputField>();
        diceType = GameObject.Find("TypeDice").transform.GetChild(0).GetComponent<Text>();
        diceMenu = GameObject.Find("DiceMenu");
        diceSwipeBar = GameObject.Find("DiceSwipeBar");
        swipeToRoll = GameObject.Find("Swipe");
        saveBanner = GameObject.Find("SaveBanner");
        diceMenu.SetActive(false);
        diceSwipeBar.SetActive(false);
        swipeToRoll.SetActive(false);
        if (PlayerPrefs.HasKey("lastFile")) {
            LoadCharacter(PlayerPrefs.GetString("lastFile"));
            return;
        }
        SetCharacterName();
        SetCharacterLevel();
        SetCharacterTraits();
        SetCharacterBioAndNotes();
        SetCharacterStr();
        SetCharacterDex();
        SetCharacterEnd();
        SetCharacterInt();
        SetCharacterCha();
        SetCharacterInu();
        SetCharacterLuc();
        SetCharacterSol();
        SetCharacterHP();
        SetCharacterMaxHP();
        SetCharacterAC();
        SetCharacterEquipment();
        SetCharacterWeapons();
        SetCharacterArmour();
        SetCharacterSpells();
        SetCharacterCandM();
        SaveCharacter();
    }

    private void Update() {
        count += Time.deltaTime;
        frameCount++;
        EverySecond = false;
        Every32Frames = false;

        if (frameCount % 32 == 0) {
            Every32Frames = true;
            frameCount = 1;
        }
        
        if (count > 1) {
            EverySecond = true;
            count = 0;
        }
    }

    private void SetUnsavedChanges(bool val) {
        UnsavedChanges = val;
        // if (val) StartCoroutine(saveBanner.GetComponent<SaveBanner>().Slide(val));
    }

    public void SetCharacterName() {
        currentCharacter.name = name.text;
        SetUnsavedChanges(true);
    }

    public void SetCharacterTraits() {
        currentCharacter.traits = traits.text;
        SetUnsavedChanges(true);
    }

    public void SetCharacterBioAndNotes() {
        currentCharacter.bioNotes = bioAndNotes.text;
        SetUnsavedChanges(true);
    }

    public void SetCharacterLevel() {
        currentCharacter.lvl = int.Parse(level.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterHP() {
        currentCharacter.hp = int.Parse(hitpoints.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterMaxHP() {
        currentCharacter.maxHp = int.Parse(maxHitpoints.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterAC() {
        currentCharacter.ac = int.Parse(ac.text); 
        SetUnsavedChanges(true);
    }

    public void SetCharacterStr() {
        currentCharacter.str = int.Parse(strength.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterDex() {
        currentCharacter.dex = int.Parse(dexterity.text); 
        SetUnsavedChanges(true);
    }

    public void SetCharacterEnd() {
        currentCharacter.end = int.Parse(endurance.text); 
        SetUnsavedChanges(true);
    }

    public void SetCharacterInt() {
        currentCharacter.intl = int.Parse(intelligence.text); 
        SetUnsavedChanges(true);
    }

    public void SetCharacterCha() {
        currentCharacter.cha = int.Parse(charisma.text); 
        SetUnsavedChanges(true);
    }

    public void SetCharacterInu() {
        currentCharacter.intu = int.Parse(intuition.text); 
        SetUnsavedChanges(true);
    }

    public void SetCharacterLuc() {
        currentCharacter.luc = int.Parse(luck.text); 
        SetUnsavedChanges(true);
    }

    public void SetCharacterSol() {
        currentCharacter.sol = int.Parse(strokesOfLuck.text); 
        SetUnsavedChanges(true);
    }

    public void SetCharacterEquipment() {
        currentCharacter.equipmentString = equipment.text; 
        SetUnsavedChanges(true);
    }

    public void SetCharacterWeapons() {
        currentCharacter.weaponString = weapons.text; 
        SetUnsavedChanges(true);
    }

    public void SetCharacterArmour() {
        currentCharacter.armourString = armour.text; 
        SetUnsavedChanges(true);
    }

    public void SetCharacterSpells() {
        currentCharacter.spellsString = spells.text; 
        SetUnsavedChanges(true);
    }

    public void SetCharacterCandM() {
        currentCharacter.cAndMString = cAndM.text; 
        SetUnsavedChanges(true);
    }

    public void SetUIName() {
        nameF.text = currentCharacter.name; 
        SetUnsavedChanges(true);
    }

    public void SetUIPortrait() {
        if (!File.Exists(currentCharacter.portraitPath)) {
            portrait.sprite = Sprite.Create(defaultPortrait, new Rect(0,0, defaultPortrait.width, defaultPortrait.height), new Vector2(0.5f, 0.5f)); 
            return;
        }
        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(currentCharacter.portraitPath));
        var smallestDim = texture.width < texture.height ? texture.width : texture.height;
        portrait.sprite = Sprite.Create(texture, 
            new Rect(0,texture.height - smallestDim, smallestDim, smallestDim), new Vector2(0.5f, 0.5f));
    }
    public void SetUITraits() { traits.text = currentCharacter.traits; }
    public void SetUIBioAndNotes() { bioAndNotes.text = currentCharacter.bioNotes; }
    public void SetUILevel() { levelF.text = currentCharacter.lvl.ToString(); }
    public void SetUIHP() { hitpointsF.text = currentCharacter.hp.ToString(); }
    public void SetUIMaxHP() { maxHitpointsF.text = currentCharacter.maxHp.ToString(); }
    public void SetUIAC() { acF.text = currentCharacter.ac.ToString(); }
    public void SetUIStr() { strengthF.text = currentCharacter.str.ToString(); }
    public void SetUIDex() { dexterityF.text = currentCharacter.dex.ToString(); }
    public void SetUIEnd() { enduranceF.text = currentCharacter.end.ToString(); }
    public void SetUIInt() { intelligenceF.text = currentCharacter.intl.ToString(); }
    public void SetUICha() { charismaF.text = currentCharacter.cha.ToString(); }
    public void SetUIInu() { intuitionF.text = currentCharacter.intu.ToString(); }
    public void SetUILuc() { luckF.text = currentCharacter.luc.ToString(); }
    public void SetUISol() { strokesOfLuck.text = currentCharacter.sol.ToString(); }
    public void SetUIEquipment() { equipment.text = currentCharacter.equipmentString; }
    public void SetUIWeapons() { weapons.text = currentCharacter.weaponString; }
    public void SetUIArmour() { armour.text = currentCharacter.armourString; }
    public void SetUISpells() { spells.text = currentCharacter.spellsString; }
    public void SetUICorruptionsMutations() { cAndM.text = currentCharacter.cAndMString; }

    public void SaveCharacter() {
        SaveSystem.Save(currentCharacter);
        SetUnsavedChanges(false);
    }

    public void LoadCharacter(string path) {
        currentCharacter = SaveSystem.Load(path);
        SetUIName();
        SetUIPortrait();
        SetUITraits();
        SetUIBioAndNotes();
        SetUILevel();
        SetUIHP();
        SetUIMaxHP();
        SetUIAC();
        SetUIStr();
        SetUIDex();
        SetUIEnd();
        SetUIInt();
        SetUICha();
        SetUIInu();
        SetUILuc();
        SetUISol();
        SetUIEquipment();
        SetUIWeapons();
        SetUIArmour();
        SetUISpells();
        SetUICorruptionsMutations();
    }

    public void ResetHamburger() {
        hamburger.value = 0;
    }

    public void ResetTextBox() {
        activeTextBox.enabled = false;
        switch (activeTextBox.gameObject.name) {
            case "TraitsText" : SetCharacterTraits(); break;
            case "BioNotesText" : SetCharacterBioAndNotes(); break;
            case "EquipmentText" : SetCharacterEquipment(); break;
            case "WeaponText" : SetCharacterWeapons(); break;
            case "ArmourText" : SetCharacterArmour(); break;
            case "SpellsText" : SetCharacterSpells(); break;
            case "CandMText" : SetCharacterCandM(); break;
        }
    }

    public void OpenDiceRoller() {
        diceMenu.SetActive(true);
        diceSwipeBar.SetActive(true);
        swipeToRoll.SetActive(true);
    }

    public void GoUpDiceChain() {
        string newNum = numDice.text, newType = diceType.text;
        switch (diceType.text) {
            case "4": newType = "6"; break;
            case "6": newType = "7"; break;
            case "7": newType = "8"; break;
            case "8": newType = "10"; break;
            case "10":
                newNum = (int.Parse(newNum) + 1).ToString();
                if (int.Parse(numDice.text) < 4) newType = (int.Parse(diceType.text) - (5 - int.Parse(numDice.text))).ToString();
                break;
            default: break;
        }
        numDice.text = newNum;
        diceType.text = newType;
    }
    
    public void GoDownDiceChain() {
        string newNum = numDice.text, newType = diceType.text;
        switch (diceType.text) {
            case "4": break;
            default:
                if (numDice.text == (int.Parse(diceType.text)-4).ToString()) {
                    newNum = (int.Parse(diceType.text) - 5).ToString();
                    newType = "10";
                } else if (diceType.text == "6" || diceType.text == "10" && int.Parse(numDice.text) < 5)
                    newType = (int.Parse(newType) - 2).ToString();
                else if (diceType.text == "10" && int.Parse(numDice.text) >= 5)
                    newNum = (int.Parse(newNum) - 1).ToString();
                else if (diceType.text == "7" || diceType.text == "8") newType = (int.Parse(newType) - 1).ToString();
                break;
        }
        numDice.text = newNum;
        diceType.text = newType;
    }

    public void PlayButtonSound() {
        mainAudio.clip = buttonPress;
        mainAudio.Play();
    }

    public void PlayRollSound() {
        mainAudio.clip = diceRoll;
        mainAudio.Play();
    }
}

[Serializable]
public class Character {
    public string name, traits, bioNotes, luckyRoll, portraitPath, equipmentString, weaponString, armourString, spellsString, cAndMString;
    public int lvl, hp, maxHp, ac, str, dex, end, intl, cha, intu, luc, sol;
    public List<string>[] equipment;
    public string[] spells, mutations, corruptions;
}

public static class SaveSystem {
    public static void Save(Character c) {
        var bf = new BinaryFormatter();
        var fName = DateTime.Now.ToString(new CultureInfo("en-GB"));
        fName = fName.Replace('/', '-').Replace(' ', '-').Replace(':', '-');
        if (!Directory.Exists(Application.persistentDataPath + "/" + c.name))
            Directory.CreateDirectory(Application.persistentDataPath + "/" + c.name);
        var path = Application.persistentDataPath + "/" + c.name + "/" + fName + ".rog";
        var file = new FileStream(path, FileMode.Create);
        
        Debug.Log("Saving " + c.name + " to " + path);
        PlayerPrefs.SetString("lastFile", path);
        bf.Serialize(file, c);
        file.Close();
    }

    public static Character Load(string path) {
        if (File.Exists(path)) {
            var bf = new BinaryFormatter();
            var file = new FileStream(path, FileMode.Open);
            var c = (Character)bf.Deserialize(file);
            file.Close();
            return c;
        }
        Debug.LogError("Save file not found in " + path);
        return null;
    }
}

