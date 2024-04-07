using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    private GameManager _gm;
    private Text _name;
    private LoadFade _loadFade;

    private void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        _name = transform.GetChild(0).GetComponent<Text>();
        _loadFade = transform.parent.parent.parent.parent.GetChild(0).GetComponent<LoadFade>();
    }

    public void LoadSelectedCharacter()
    {
        var path = Application.persistentDataPath + "/" + _name.text;
        var directory = new DirectoryInfo(path);
        path += "/" + directory.GetFiles().OrderByDescending(f => f.Name).ElementAt(1).Name;
        print(path);
        _gm.LoadCharacter(path);
        PlayerPrefs.SetString("lastFile", path);
        _loadFade.Disappear();
    }

    public void DeleteSelectedCharacter()
    {
        var path = Application.persistentDataPath + "/" + _name.text;
        Directory.Delete(path, true);
        _name.text = "Deleted!";
        Destroy(transform.GetChild(1).gameObject);
    }
}
