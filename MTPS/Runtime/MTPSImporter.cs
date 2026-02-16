using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        [Tooltip("Search prefabs for TextPack Clients to import keys from them")]
        [SerializeField]
        private GameObject[] Prefabs;

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

        [Tooltip("Use key name as key value")]
        [SerializeField]
        private bool DuplicateKeysForValues;

        private List<string> keys;
        private Dictionary<string, string> keysdict;
        private string jsonstring;
        private Dictionary<string, object> jsonkeys;

        private void Start()
        {
            keys = new List<string>();
            foreach (string Scene in Scenes)
            {
                SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
            }
            foreach (GameObject prefab in Prefabs)
            {
                Instantiate(prefab);
            }
            foreach (TextAsset Json in Jsons)
            {
                jsonkeys = JsonConvert.DeserializeObject<Dictionary<string, object>>(Json.text);
                foreach (var entry in jsonkeys)
                {
                    if (entry.Value is JArray list)
                        keys.AddRange(list.ToObject<List<string>>());
                    else
                        keys.Add(entry.Value.ToString());
                }
                //keys.AddRange(jsonkeys.Keys.ToList());    TODO: Option to import object's keys
            }
            Invoke(nameof(CreateTemplate), DelayInSeconds);
        }

        private void CreateTemplate()
        {
            keysdict = new Dictionary<string, string> { };
            foreach (
                ITextPackClient text in FindObjectsByType<MonoBehaviour>(
                        FindObjectsInactive.Include,
                        FindObjectsSortMode.None
                    )
                    .OfType<ITextPackClient>()
                    .ToArray()
            )
                keys.AddRange(text.GetKeys());
            keys.AddRange(ExplicitKeys.ToList());
            keys = keys.Distinct().ToList();
            foreach (string key in keys)
                keysdict.Add(key, DuplicateKeysForValues ? key : "");
            jsonstring = JsonConvert.SerializeObject(keysdict, Formatting.Indented);
            string filePath = Path.Combine(Application.dataPath, "Template.json");
            File.WriteAllText(filePath, jsonstring);
            Debug.LogWarning("JSON Template saved to: " + filePath);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }
}
