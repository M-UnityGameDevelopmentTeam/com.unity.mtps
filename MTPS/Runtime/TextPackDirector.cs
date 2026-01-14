using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace MTPS
{
    [Serializable]
    public struct TextPack
    {
        public string Name;
        public string FilePath;
    }

    public class TextPackDirector : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<string, string> CurrentTextPack;

        [SerializeField]
        private bool DestroyOnLoad;
        public List<TextPack> TextPacks;
        public List<TextPack> FTextPacks => TextPacks;
        public Dictionary<string, string> FCurrentTextPack => CurrentTextPack;
        private TextPack tempPack;

        private void Awake()
        {
            if (gameObject.scene.buildIndex != -1 && !DestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            string targetFolderPath = Application.dataPath + "/TextPacks";
            if (Directory.Exists(targetFolderPath))
            {
                string[] files = Directory.GetFiles(targetFolderPath);
                foreach (string filePath in files)
                {
                    if (Path.GetExtension(filePath) == ".json")
                    {
                        tempPack.Name = Path.GetFileNameWithoutExtension(filePath);
                        tempPack.FilePath = filePath;
                        TextPacks.Add(tempPack);
                    }
                }
            }
            if (!File.Exists(PlayerPrefs.GetString("CurrentTextPackPath")))
                print(Application.systemLanguage);
            PlayerPrefs.SetString(
                "CurrentTextPackPath",
                TextPacks.Find(s => s.Name.Contains(Application.systemLanguage.ToString())).FilePath
            );
            CurrentTextPack = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                File.ReadAllText(
                    PlayerPrefs.GetString("CurrentTextPackPath", TextPacks[0].FilePath)
                )
            );
        }

        public void ChangeTextPack(int Index)
        {
            if (File.Exists(TextPacks[Index].FilePath))
            {
                CurrentTextPack = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    File.ReadAllText(TextPacks[Index].FilePath)
                );
                PlayerPrefs.SetString("CurrentTextPackPath", TextPacks[Index].FilePath);
            }
            foreach (
                TextPackTMPClient text in FindObjectsByType<TextPackTMPClient>(
                    FindObjectsSortMode.None
                )
            )
            {
                text.UpdateText();
            }
        }
    }
}
