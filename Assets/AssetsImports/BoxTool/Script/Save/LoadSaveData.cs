using System;
using UnityEngine;
using System.IO;
using UnityEngine.Serialization;

namespace BT.Save
{
    public class LoadSaveData : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RSE_Event rseCommandLoad;
        [SerializeField] private RSE_Event rseCommandSave;
        [SerializeField] private RSE_Event rseCommandClear;
        [SerializeField] private RSO_ContentSaved rsoContentSaved;

        private string filepath;

        private void OnEnable()
        {
            rseCommandLoad.action += LoadFromJson;
            rseCommandSave.action += SaveToJson;
            rseCommandClear.action += ClearContent;
        }

        private void OnDisable()
        {
            rseCommandLoad.action -= LoadFromJson;
            rseCommandSave.action -= SaveToJson;
        }

        private void Start()
        {
            filepath = Application.persistentDataPath + "/Save.json";

            if (FileAlreadyExist()) LoadFromJson();
            else
            {
                rsoContentSaved.Value = new ContentSaved();
                SaveToJson();
            }
        }

        private void SaveToJson()
        {
            string infoData = JsonUtility.ToJson(rsoContentSaved.Value);
            File.WriteAllText(filepath, infoData);
        }

        private void LoadFromJson()
        {
            string infoData = System.IO.File.ReadAllText(filepath);
            rsoContentSaved.Value = JsonUtility.FromJson<ContentSaved>(infoData);
            if (rsoContentSaved.Value == null) rsoContentSaved.Value = new ContentSaved();
        } 

        private void ClearContent()
        {
            rsoContentSaved.Value = new();
            SaveToJson();
        }

        private bool FileAlreadyExist()
        {
            return File.Exists(filepath);
        }
        
    }   
}
