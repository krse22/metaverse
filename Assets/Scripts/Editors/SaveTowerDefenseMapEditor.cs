using Prototyping.Games;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SaveTowerDefenseMapEditor : OdinEditorWindow
{

    public static SaveTowerDefenseMapEditor Instance;

    [MenuItem("My Game/My Window")]
    public static void OpenWindow(TowerDefenseMapMaker mapMaker)
    {
        Init();
        Instance.mapMaker = mapMaker;
    }

    public static void Init()
    {
        Instance = GetWindow<SaveTowerDefenseMapEditor>();
        Instance.Show();
    }

    public TowerDefenseMapMaker mapMaker;
    public string savePath = "Assets/Prototyping/Minigames/Tower Defense/Prefabs/Maps";
    public string mapName;

    [Button("Save Field")]
    public void SaveField()
    {
        if (!Instance) Init();

        if (!Validate()) return;

        // Since you can't directly remove script from a prefab
        GameObject duplicate = Instantiate(mapMaker.gameObject);
        DestroyImmediate(duplicate.GetComponent<TowerDefenseMapMaker>());
        duplicate.transform.name = "MAP";

        PrefabUtility.SaveAsPrefabAsset(duplicate, $"{savePath}/{mapName}.prefab", out bool prefabSuccess);

        DestroyImmediate(duplicate);

        if (prefabSuccess == true)
        {
            Instance.Close();
        } else
        {
            Debug.LogError("Failed to save map");
        }

    }

    bool Validate()
    {
        if (!Directory.Exists(savePath))
        {
            Debug.LogError("Folder doesn't exist, check the path or change it");
            return false;
        }

        if (!mapMaker)
        {
            Debug.LogError("Map Maker instance doesn't exist, create one or assign it in inspector");
            return false;
        }

        if (mapName == "")
        {
            Debug.LogError("Map name can't be empty");
            return false;
        }

        if (File.Exists($"{savePath}/{mapName}.prefab"))
        {
            Debug.LogError("Map with the same name already exists");
            return false;
        }

        return true;
    }


}
