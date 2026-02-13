using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private TextPack tempPack;

        private void Awake()
        {
            if (gameObject.scene.buildIndex != -1 && !DestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            TextPacks = LoadTextPacksFromFolder(Application.dataPath + "/TextPacks");
            if (TextPacks.Count == 0)
            {
                CurrentTextPack = new Dictionary<string, string>();
                return;
            }
            tempPack = TextPacks.Find(s =>
                s.FilePath == (PlayerPrefs.GetString("CurrentTextPackPath", TextPacks[0].FilePath))
            );
            if (tempPack.Name == null)
            {
                tempPack = TextPacks.Find(s =>
                    s.Name.Contains(Application.systemLanguage.ToString())
                );
                if (tempPack.Name == null)
                    tempPack = TextPacks[0];
                PlayerPrefs.SetString("CurrentTextPackPath", tempPack.FilePath);
            }
            CurrentTextPack = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                File.ReadAllText(tempPack.FilePath)
            );
        }

        public List<TextPack> LoadTextPacksFromFolder(string FolderPath)
        {
            TextPacks = new List<TextPack>();
            if (!Directory.Exists(FolderPath))
                return TextPacks;
            string[] files = Directory.GetFiles(FolderPath);
            foreach (string filePath in files)
            {
                try
                {
                    JToken.Parse(File.ReadAllText(filePath));
                    tempPack.Name = Path.GetFileNameWithoutExtension(filePath);
                    tempPack.FilePath = filePath;
                    TextPacks.Add(tempPack);
                }
                catch { }
            }
            tempPack.Name = null;
            tempPack.FilePath = null;
            return TextPacks;
        }

        public string GetKeyFromTextPack(string Key)
        {
            if (CurrentTextPack.ContainsKey(Key))
                return CurrentTextPack[Key];
            return Key;
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
                ITextPackClient client in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                    .OfType<ITextPackClient>()
                    .ToArray()
            )
            {
                client.TextPackUpdate();
            }
        }
    }
}
