using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MTPS
{
    public class MTPSImporter : MonoBehaviour
    {
        [Header("Import keys from:")]
        [Space]
        [Tooltip("Search scenes for TextPack Clients to import keys from them")]
        [SerializeField]
        private string[] Scenes;

        [Tooltip("Import keys from JSON files")]
        [SerializeField]
        private TextAsset[] Jsons;

        [Tooltip("Amount of seconds to wait before searching for TextPack Clients in scenes")]
        [SerializeField]
        private float DelayInSeconds = 1;

        [Space]
        [Tooltip("Override custom keys")]
        [SerializeField]
        private string[] ExplicitKeys;

        private List<string> keys;
        private string jsonstring;
        private Dictionary<string, string> jsonkeys;

        private void Start()
        {
            keys = new List<string>();
            foreach (string Scene in Scenes)
            {
                SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
            }
            foreach (TextAsset Json in Jsons)
            {
                jsonkeys = JsonConvert.DeserializeObject<Dictionary<string, string>>(Json.text);
                print(jsonkeys.Keys.ToList());
                keys.AddRange(jsonkeys.Keys.ToList());
            }
            Invoke(nameof(CreateTemplate), DelayInSeconds);
        }

        private void CreateTemplate()
        {
            jsonstring = "{\n";
            foreach (
                TextPackTMPClient text in FindObjectsByType<TextPackTMPClient>(
                    FindObjectsSortMode.None
                )
            )
                if (!keys.Contains(text.Key))
                    keys.Add(text.Key);
            keys.AddRange(ExplicitKeys.ToList());
            keys = keys.Distinct().ToList();
            foreach (string key in keys)
                jsonstring += $"    \"{key}\": \"{key}\",\n";
            jsonstring += "}";
            string filePath = Path.Combine(Application.dataPath, "Template.json");
            File.WriteAllText(filePath, jsonstring);
            Debug.LogWarning("JSON file saved to: " + filePath);
        }
    }
}
