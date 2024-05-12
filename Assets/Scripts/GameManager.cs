using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static string repoURL =
            "https://raw.githubusercontent.com/ckwk/rogue-character-sheet/main/API/",
        directoryFile = "Directory.ini";
    public static bool EveryOtherFrame,
        EveryFourFrames,
        EveryEightFrames,
        Every32Frames,
        EverySecond;
    public static bool UnsavedChanges;
    public static int currentScreen;
    private float count;
    private int frameCount;
    public static List<TSV> spellObjects = new List<TSV>(),
        mutationsObjects = new List<TSV>(),
        traitObjects = new List<TSV>();
    public static LoadedModules loadedModules = new LoadedModules();
    private GameObject diceMenu,
        diceSwipeBar,
        swipeToRoll,
        diceButton,
        saveBanner,
        statsPage,
        characterPage,
        equipmentPage,
        magicPage;
    public GameObject powerScreen,
        textBox,
        textBoxActivator,
        deleteScreen,
        deleteScreenActivator;
    private AudioSource mainAudio;
    public List<GameObject> pages;
    public List<Text> navText;
    public List<Image> navIcons;
    public AudioClip buttonPress,
        diceRoll;
    public Character currentCharacter;
    public static List<string> activeModules = new List<string>();
    public TMP_Text txtName;
    public InputField inField,
        numDice,
        powerScreenResultNum;
    public TextBox activeTextBox;
    private Dropdown diceType;
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
        spellStat,
        mutationStat;
    public TMP_Text powerScreenName,
        powerScreenCost,
        powerScreenDuration,
        powerScreenCastingTime,
        powerScreenResult;

    public HandleEntries equipment,
        weapons,
        armour,
        spells,
        mutations,
        corruptions,
        traits,
        notes;
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

    private void Awake()
    {
        Application.targetFrameRate = 90;
        currentCharacter = new Character();
        UnsavedChanges = false;
    }

    private void Start()
    {
        mainAudio = Camera.main.GetComponent<AudioSource>();
        numDice = GameObject.Find("NumDice").GetComponent<InputField>();
        diceType = GameObject.Find("TypeDice").GetComponent<Dropdown>();
        diceMenu = GameObject.Find("DiceMenu");
        diceSwipeBar = GameObject.Find("DiceSwipeBar");
        swipeToRoll = GameObject.Find("Swipe");
        diceButton = GameObject.Find("DiceButton");
        statsPage = GameObject.Find("Stats");
        characterPage = GameObject.Find("Character");
        equipmentPage = GameObject.Find("Equipment");
        magicPage = GameObject.Find("Magic");
        powerScreen = GameObject.Find("PowerScreen");
        saveBanner = GameObject.Find("SaveBanner");
        diceMenu.SetActive(false);
        diceSwipeBar.SetActive(false);
        swipeToRoll.SetActive(false);
        characterPage.SetActive(false);
        equipmentPage.SetActive(false);
        magicPage.SetActive(false);
        powerScreen.SetActive(false);
        SaveSystem.ImportDownloadedModules();
        if (PlayerPrefs.HasKey("lastFile"))
        {
            StartCoroutine(SaveSystem.ImportModules(PlayerPrefs.GetString("lastFile"), false));
            LoadCharacter(PlayerPrefs.GetString("lastFile"));
            saveBanner.GetComponent<SaveBanner>().Disappear();
            return;
        }
        SetCharacterName();
        SetCharacterLevel();
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
        SetCharacterTraits();
        SetCharacterBioAndNotes();
        SetCharacterEquipment();
        SetCharacterWeapons();
        SetCharacterArmour();
        SetCharacterSpells();
        SetCharacterCandM();
        SetCharacterMutations();
        SetCharacterSpellStat();
        SetCharacterMutationStat();
        SaveCharacter();
        StartCoroutine(SaveSystem.ImportModules(PlayerPrefs.GetString("lastFile"), false));
    }

    private void Update()
    {
        count += Time.deltaTime;
        frameCount++;
        EverySecond = false;
        Every32Frames = false;

        if (frameCount % 32 == 0)
        {
            Every32Frames = true;
            frameCount = 1;
        }

        if (count > 1)
        {
            EverySecond = true;
            count = 0;
        }
    }

    // set to saveBanner.Appear() for non auto-save
    private void SetUnsavedChanges(bool val)
    {
        UnsavedChanges = val;
        if (val)
            saveBanner.GetComponent<SaveBanner>().Save();
    }

    public void SetCharacterName()
    {
        print(currentCharacter.name);
        if (txtName.text.Trim('\u200B') == currentCharacter.name)
            return;
        currentCharacter.name = txtName.text.Trim('\u200B');
        SetUnsavedChanges(true);
    }

    public void SetCharacterTraits()
    {
        currentCharacter.traits = traits.serializedEntries;
        SetUnsavedChanges(true);
    }

    public void SetCharacterBioAndNotes()
    {
        currentCharacter.notes = notes.serializedEntries;
        SetUnsavedChanges(true);
    }

    public void SetCharacterLevel()
    {
        if (int.Parse(level.text) == currentCharacter.lvl)
            return;
        currentCharacter.lvl = int.Parse(level.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterHP()
    {
        if (int.Parse(hitpoints.text) == currentCharacter.hp)
            return;
        currentCharacter.hp = int.Parse(hitpoints.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterMaxHP()
    {
        if (int.Parse(maxHitpoints.text) == currentCharacter.maxHp)
            return;
        currentCharacter.maxHp = int.Parse(maxHitpoints.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterAC()
    {
        if (int.Parse(ac.text) == currentCharacter.ac)
            return;
        currentCharacter.ac = int.Parse(ac.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterStr()
    {
        if (int.Parse(strength.text) == currentCharacter.str)
            return;
        currentCharacter.str = int.Parse(strength.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterDex()
    {
        if (int.Parse(dexterity.text) == currentCharacter.dex)
            return;
        currentCharacter.dex = int.Parse(dexterity.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterEnd()
    {
        if (int.Parse(endurance.text) == currentCharacter.end)
            return;
        currentCharacter.end = int.Parse(endurance.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterInt()
    {
        if (int.Parse(intelligence.text) == currentCharacter.intl)
            return;
        currentCharacter.intl = int.Parse(intelligence.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterCha()
    {
        if (int.Parse(charisma.text) == currentCharacter.cha)
            return;
        currentCharacter.cha = int.Parse(charisma.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterInu()
    {
        if (int.Parse(intuition.text) == currentCharacter.intu)
            return;
        currentCharacter.intu = int.Parse(intuition.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterLuc()
    {
        if (int.Parse(luck.text) == currentCharacter.luc)
            return;
        currentCharacter.luc = int.Parse(luck.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterSol()
    {
        if (int.Parse(strokesOfLuck.text) == currentCharacter.sol)
            return;
        currentCharacter.sol = int.Parse(strokesOfLuck.text);
        SetUnsavedChanges(true);
    }

    public void SetCharacterEquipment()
    {
        currentCharacter.equipment = equipment.serializedEntries;
        SetUnsavedChanges(true);
    }

    public void SetCharacterWeapons()
    {
        currentCharacter.weapons = weapons.serializedEntries;
        SetUnsavedChanges(true);
    }

    public void SetCharacterArmour()
    {
        currentCharacter.armour = armour.serializedEntries;
        SetUnsavedChanges(true);
    }

    public void SetCharacterSpells()
    {
        currentCharacter.spells = spells.serializedEntries;
        SetUnsavedChanges(true);
    }

    public void SetCharacterCandM()
    {
        currentCharacter.corruptions = corruptions.serializedEntries;
        SetUnsavedChanges(true);
    }

    public void SetCharacterMutations()
    {
        currentCharacter.mutations = mutations.serializedEntries;
        SetUnsavedChanges(true);
    }

    public void SetCharacterSpellStat()
    {
        currentCharacter.spellStat = spellStat.text;
        SetUnsavedChanges(true);
    }

    public void SetCharacterMutationStat()
    {
        currentCharacter.mutationStat = mutationStat.text;
        SetUnsavedChanges(true);
    }

    public void SetUIName()
    {
        nameF.text = currentCharacter.name;
        SetUnsavedChanges(true);
    }

    public void SetUIPortrait()
    {
        if (!File.Exists(currentCharacter.portraitPath))
        {
            portrait.sprite = Sprite.Create(
                defaultPortrait,
                new Rect(0, 0, defaultPortrait.width, defaultPortrait.height),
                new Vector2(0.5f, 0.5f)
            );
            return;
        }
        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(currentCharacter.portraitPath));
        var smallestDim = texture.width < texture.height ? texture.width : texture.height;
        portrait.sprite = Sprite.Create(
            texture,
            new Rect(0, texture.height - smallestDim, smallestDim, smallestDim),
            new Vector2(0.5f, 0.5f)
        );
    }

    public void SetUITraits()
    {
        traits.serializedEntries = new List<string>(
            currentCharacter.traits.Where(entry => entry != "||||")
        );
        traits.LoadEntries();
    }

    public void SetUIBioAndNotes()
    {
        notes.serializedEntries = new List<string>(
            currentCharacter.notes.Where(entry => entry != "||||")
        );
        notes.LoadEntries();
    }

    public void SetUILevel()
    {
        levelF.text = currentCharacter.lvl.ToString();
    }

    public void SetUIHP()
    {
        hitpointsF.text = currentCharacter.hp.ToString();
    }

    public void SetUIMaxHP()
    {
        maxHitpointsF.text = currentCharacter.maxHp.ToString();
    }

    public void SetUIAC()
    {
        acF.text = currentCharacter.ac.ToString();
    }

    public void SetUIStr()
    {
        strengthF.text = currentCharacter.str.ToString();
    }

    public void SetUIDex()
    {
        dexterityF.text = currentCharacter.dex.ToString();
    }

    public void SetUIEnd()
    {
        enduranceF.text = currentCharacter.end.ToString();
    }

    public void SetUIInt()
    {
        intelligenceF.text = currentCharacter.intl.ToString();
    }

    public void SetUICha()
    {
        charismaF.text = currentCharacter.cha.ToString();
    }

    public void SetUIInu()
    {
        intuitionF.text = currentCharacter.intu.ToString();
    }

    public void SetUILuc()
    {
        luckF.text = currentCharacter.luc.ToString();
    }

    public void SetUISol()
    {
        strokesOfLuck.text = currentCharacter.sol.ToString();
    }

    public void SetUIEquipment()
    {
        equipment.serializedEntries = new List<string>(
            currentCharacter.equipment.Where(entry => entry != "||||")
        );
        equipment.LoadEntries();
    }

    public void SetUIWeapons()
    {
        weapons.serializedEntries = new List<string>(
            currentCharacter.weapons.Where(entry => entry != "||||")
        );
        weapons.LoadEntries();
    }

    public void SetUIArmour()
    {
        armour.serializedEntries = new List<string>(
            currentCharacter.armour.Where(entry => entry != "||||")
        );
        armour.LoadEntries();
    }

    public void SetUISpells()
    {
        spells.serializedEntries = new List<string>(
            currentCharacter.spells.Where(entry => entry != "||||")
        );
        spells.LoadEntries();
    }

    public void SetUICorruptionsMutations()
    {
        corruptions.serializedEntries = currentCharacter.corruptions;
        corruptions.LoadEntries();
        mutations.serializedEntries = currentCharacter.mutations;
        mutations.LoadEntries();
    }

    public void SetUISpellStat()
    {
        spellStat.text = currentCharacter.spellStat;
    }

    public void SetUIMutationStat()
    {
        mutationStat.text = currentCharacter.mutationStat;
    }

    public void SaveCharacter()
    {
        SaveSystem.Save(currentCharacter);
        SetUnsavedChanges(false);
    }

    public void LoadCharacter(string path)
    {
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
        SetUISpellStat();
        SetUIMutationStat();
        SetUnsavedChanges(false);
        saveBanner.GetComponent<SaveBanner>().Disappear();
    }

    public void ResetHamburger()
    {
        hamburger.value = 0;
    }

    public void UpdateEntryFromTextBox()
    {
        var currentEntryField = textBoxActivator.GetComponent<EntryField>();
        currentEntryField.gameObject.GetComponent<InputField>().text = textBox.transform
            .GetChild(1)
            .GetComponent<InputField>()
            .text;
        currentEntryField.UpdateHandler(true);
    }

    public void DeleteButton()
    {
        deleteScreenActivator.GetComponent<HandleEntries>().DeleteEntry();
        deleteScreen.SetActive(false);
    }

    public void OpenDiceRoller()
    {
        diceMenu.SetActive(true);
        diceSwipeBar.SetActive(true);
        swipeToRoll.SetActive(true);
        diceButton.SetActive(false);
    }

    public void GoUpDiceChain()
    {
        var newNum = numDice.text;
        var newType = diceType.value;
        switch (diceType.value)
        {
            case 0:
                newType = 1;
                break;
            case 1:
                newType = 2;
                break;
            case 2:
                newType = 3;
                break;
            case 3:
                newType = 4;
                break;
            case 4:
                newNum = (int.Parse(newNum) + 1).ToString();
                if (int.Parse(numDice.text) < 4)
                    newType = 10 - (5 - int.Parse(numDice.text)) - 5;
                break;
            default:
                break;
        }
        numDice.text = newNum;
        diceType.value = newType;
    }

    public void GoDownDiceChain()
    {
        var newNum = numDice.text;
        var newType = diceType.value;
        switch (diceType.value)
        {
            case 0:
                break;
            default:
                if (numDice.text == (diceType.value + 1).ToString())
                {
                    newNum = diceType.value.ToString();
                    newType = 4;
                }
                else if (diceType.value == 1 || diceType.value == 4 && int.Parse(numDice.text) < 5)
                    newType--;
                else if (diceType.value == 4 && int.Parse(numDice.text) >= 5)
                    newNum = (int.Parse(newNum) - 1).ToString();
                else if (diceType.value == 2 || diceType.value == 3)
                    newType--;
                break;
        }
        numDice.text = newNum;
        diceType.value = newType;
    }

    public string GetPowerResultText(TSV power, int roll)
    {
        var previousBoundary = 1;
        var currentBoundary = 1;
        foreach (var row in power.GetProperties().Skip(2))
        {
            if (row == power.GetProperties().Last())
                return string.Format("({0})\t{1}", row[0], row[1]);

            currentBoundary = int.Parse(row[0].Split('-').Last());
            if (roll <= currentBoundary)
            {
                var effect = previousBoundary == 1 ? GetEffectWithCorruption(power) : row[1];
                return string.Format("({0})\t{1}", row[0], effect);
            }
            previousBoundary = currentBoundary + 1;
        }
        return "";
    }

    private string GetEffectWithCorruption(TSV power)
    {
        var formattedCorruption = power.GetProperties()[1][1]
            .Replace(": ", ":\n\n")
            .Replace("; ", ";\n\n");
        return string.Format("{0}\n\n{1}", power.GetProperties()[2][1], formattedCorruption);
    }

    public void PlayButtonSound()
    {
        mainAudio.clip = buttonPress;
        mainAudio.Play();
    }

    public void PlayRollSound()
    {
        mainAudio.clip = diceRoll;
        mainAudio.Play();
    }
}

[Serializable]
public class NestedList
{
    public List<string> properties;
}

[Serializable]
public class Character
{
    public string name,
        luckyRoll,
        portraitPath,
        spellStat,
        mutationStat;
    public int lvl,
        hp,
        maxHp,
        ac,
        str,
        dex,
        end,
        intl,
        cha,
        intu,
        luc,
        sol;
    public List<string> equipment,
        weapons,
        armour,
        spells,
        mutations,
        corruptions,
        traits,
        notes,
        modules;
}

public static class SaveSystem
{
    private static Dictionary<string, List<string>> _modules =
        new Dictionary<string, List<string>>();
    private static List<string> _spells = new List<string>(),
        _mutations = new List<string>(),
        _traits = new List<string>();

    public static void Save(Character c)
    {
        var bf = new BinaryFormatter();
        var fName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        var directory = Application.persistentDataPath + "/" + c.name;
        var path = directory + "/" + fName + ".rog";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            SaveActiveModules(path);
        }
        var file = new FileStream(path, FileMode.Create);

        Debug.Log("Saving " + c.name + " to " + path);
        PlayerPrefs.SetString("lastFile", path);
        bf.Serialize(file, c);
        file.Close();

        //Delete up to max files
        var maxFiles = 21;
        var filesInDir = Directory.GetFiles(directory);
        Array.Sort(filesInDir);
        if (filesInDir.Length <= maxFiles)
            return;
        File.Delete(filesInDir[0]);
    }

    public static Character Load(string path)
    {
        if (File.Exists(path))
        {
            var bf = new BinaryFormatter();
            var file = new FileStream(path, FileMode.Open);
            var c = (Character)bf.Deserialize(file);
            file.Close();

            return c;
        }
        Debug.LogError("Save file not found in " + path);
        return null;
    }

    public static void SaveActiveModules(string path)
    {
        var bf = new BinaryFormatter();
        var modulesPath = Path.GetDirectoryName(path).Replace("\\", "/") + "/modules.rog";
        var modulesFile = new FileStream(modulesPath, FileMode.OpenOrCreate);
        bf.Serialize(modulesFile, GameManager.activeModules);
        modulesFile.Close();
    }

    public static void LoadActiveModules(string path)
    {
        var modulesPath = Path.GetDirectoryName(path).Replace("\\", "/") + "/modules.rog";
        if (File.Exists(modulesPath))
        {
            var bf = new BinaryFormatter();
            var modulesFile = new FileStream(modulesPath, FileMode.Open);
            GameManager.activeModules = (List<string>)bf.Deserialize(modulesFile);
            modulesFile.Close();
        }
    }

    public static void SerializeDownloadedModules(LoadedModules loadedModules)
    {
        var bf = new BinaryFormatter();
        var file = new FileStream(
            Application.persistentDataPath + "/downloadedModules.rog",
            FileMode.OpenOrCreate
        );
        Debug.Log("Serializing downloaded modules...");
        bf.Serialize(file, loadedModules);
        file.Close();
    }

    public static void ImportDownloadedModules()
    {
        var path = Application.persistentDataPath + "/downloadedModules.rog";
        if (File.Exists(path))
        {
            var bf = new BinaryFormatter();
            var file = new FileStream(path, FileMode.Open);
            GameManager.loadedModules = (LoadedModules)bf.Deserialize(file);
            file.Close();
            return;
        }
        Debug.LogError("No downloaded modules");
    }

    private static int GetIndexOfComponent(List<string> list, string component)
    {
        var index = 0;
        foreach (var comp in list)
        {
            if (comp.Split('\n')[0].Trim() == component.Split('.')[0])
                return index;
            //Debug.Log(comp.Split('\n')[0].Trim() + " does not match " + component.Split('.')[0]);
            index++;
        }
        return -1;
    }

    public static IEnumerator ImportModules(string path, bool forceDownload)
    {
        Debug.Log("fetching API");
        var loadedModuleNames = new List<string>();
        foreach (var m in GameManager.loadedModules.modules)
        {
            loadedModuleNames.Add(m.Split(',')[0]);
        }
        using (
            UnityWebRequest directoryRequest = UnityWebRequest.Get(
                GameManager.repoURL + GameManager.directoryFile
            )
        )
        {
            yield return directoryRequest.SendWebRequest();
            if (directoryRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(directoryRequest.error);
            }
            else
            {
                var directory = directoryRequest.downloadHandler.text.Replace("\r\n", "\n");

                // compose our dictionary of modules
                var directoryArray = directory.Split('\n');
                _modules.Add(directoryArray[0], new List<string>());
                for (var i = 1; i < directoryArray.Length; i++)
                {
                    if (directoryArray[i].StartsWith("[") || directoryArray[i] == "")
                        continue; //TODO: expand for more module types
                    _modules[directoryArray[0]].Add(directoryArray[i]);
                }

                // download some modules
                foreach (var section in _modules)
                {
                    foreach (var module in section.Value)
                    {
                        if (loadedModuleNames.Contains(module) && !forceDownload)
                            continue;
                        Debug.Log("fetching " + module);
                        loadedModuleNames.Add(module);
                        yield return FetchModule(module);
                    }
                }

                // load the components from the downloaded modules
                yield return LoadComponents(_spells, "/Spells/", GameManager.loadedModules.spells);
                yield return LoadComponents(
                    _mutations,
                    "/Mutations/",
                    GameManager.loadedModules.mutations
                );
                yield return LoadComponents(_traits, "/Traits/", GameManager.loadedModules.traits);
            }
        }

        // load the character's modules.rog file if it exists
        LoadActiveModules(path);

        if (GameManager.activeModules.Count == 0)
            GameManager.activeModules.Add(GameManager.loadedModules.modules[0]);
        Debug.Log(GameManager.activeModules[0]);

        // activate modules for this character
        foreach (var module in GameManager.activeModules)
        {
            var componentList = GameManager.loadedModules.modules
                .Find(s => s.Contains(module))
                .Split(',');
            var source = GameManager.loadedModules.spells;
            var target = GameManager.spellObjects;
            foreach (var component in componentList.Skip(1)) // skips over the module's name, i.e. Core.ini
            {
                var componentName = component.Split('.')[0];
                switch (component)
                {
                    case "[Mutations]":
                        source = GameManager.loadedModules.mutations;
                        target = GameManager.mutationsObjects;
                        break;
                    case "[Traits]":
                        source = GameManager.loadedModules.traits;
                        target = GameManager.traitObjects;
                        break;
                    default:
                        ActivateComponent(source[GetIndexOfComponent(source, component)], target);
                        break;
                }
            }
        }

        SaveActiveModules(path);
        SerializeDownloadedModules(GameManager.loadedModules);
    }

    static IEnumerator FetchModule(string module)
    {
        using (UnityWebRequest moduleRequest = UnityWebRequest.Get(GameManager.repoURL + module))
        {
            yield return moduleRequest.SendWebRequest();
            if (moduleRequest.result != UnityWebRequest.Result.Success)
                Debug.Log(moduleRequest.error);
            else
            {
                var m = moduleRequest.downloadHandler.text.Replace("\r\n", "\n");

                // compose our dictionary of spells, mutations, and traits
                var moduleArray = m.Split('\n');
                bool mutations = false,
                    traits = false;
                for (var i = 1; i < moduleArray.Length; i++)
                {
                    module = moduleArray[i] == "" ? module : module + "," + moduleArray[i];
                    switch (moduleArray[i].Trim())
                    {
                        case "":
                            continue;
                        case "[Traits]":
                            traits = true;
                            continue;
                        case "[Mutations]":
                            mutations = true;
                            continue;
                    }

                    if (traits)
                        _traits.Add(moduleArray[i]);
                    else if (mutations)
                        _mutations.Add(moduleArray[i]);
                    else
                        _spells.Add(moduleArray[i]);
                    Debug.Log("entered " + moduleArray[i]);
                }
                GameManager.loadedModules.modules.Add(module);
            }
        }
    }

    static IEnumerator LoadComponents(List<string> type, string folder, List<string> target)
    {
        foreach (var item in type)
        {
            using var request = UnityWebRequest.Get(GameManager.repoURL + folder + item);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
                Debug.Log(request.error);
            else
                target.Add(request.downloadHandler.text);
        }
    }

    static void ActivateComponent(string component, List<TSV> activatedList)
    {
        var componentToActivate = TSV.ParseTSV(component.Replace("\r\n", "\n"));
        activatedList.Add(componentToActivate);
        Debug.Log("Activated " + componentToActivate.GetName());
    }
}

public class TSV
{
    private string name;
    private List<string[]> properties;

    public TSV(string name)
    {
        this.name = name;
        properties = new List<string[]>();
    }

    public string GetName()
    {
        return name;
    }

    public List<string[]> GetProperties()
    {
        return properties;
    }

    public void AddToProperties(string[] property)
    {
        properties.Add(property);
    }

    public static TSV ParseTSV(string tsv)
    {
        var tsvArray = tsv.Split('\n');
        TSV tsvOut = new TSV(tsvArray[0].TrimEnd('\t'));
        for (int i = 1; i < tsvArray.Length; i++)
        {
            tsvOut.AddToProperties(tsvArray[i].TrimEnd('\t').Split('\t'));
        }

        return tsvOut;
    }
}

[Serializable]
public class LoadedModules
{
    public List<string> modules = new List<string>(),
        spells = new List<string>(),
        mutations = new List<string>(),
        traits = new List<string>();
}
