using System;
using UnityEngine;
using System.IO;

namespace BT.Save
{
    public class LoadSaveData : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RSE_Event OnCommandLoad;
        [SerializeField] private RSE_Event OnCommandSave;
        [SerializeField] private RSE_Event OnCommandClear;
        [SerializeField] private RSE_GetDataSave OnGetDataSave;
        
        private ContentSaved contentSaved = new ();
        private string filepath;

        private void OnEnable()
        {
            OnCommandLoad.action += LoadFromJson;
            OnCommandSave.action += SaveToJson;
            OnCommandClear.action += ClearContent;
            OnGetDataSave.action += GetData;
        }

        private void OnDisable()
        {
            OnCommandLoad.action -= LoadFromJson;
            OnCommandSave.action -= SaveToJson;
        }

        private void Start()
        {
            filepath = Application.persistentDataPath + "/Save.json";

            if (FileAlreadyExist()) LoadFromJson();
            else SaveToJson();
        }

        private void SaveToJson()
        {
            string infoData = JsonUtility.ToJson(contentSaved);
            File.WriteAllText(filepath, infoData);
        }

        private void LoadFromJson()
        {
            string infoData = System.IO.File.ReadAllText(filepath);
            contentSaved = JsonUtility.FromJson<ContentSaved>(infoData);
        }

        private void ClearContent()
        {
            contentSaved = new();
            SaveToJson();
        }

        private bool FileAlreadyExist()
        {
            return File.Exists(filepath);
        }

        private ContentSaved GetData()
        {
            return contentSaved;
        }
    }   
}
