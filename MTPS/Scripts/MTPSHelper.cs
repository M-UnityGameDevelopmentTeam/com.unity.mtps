using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
public class MTPSHelper : MonoBehaviour
{
    [SerializeField] private string[] scenes;
    [SerializeField] private string[] ExplicitKeys;
    private List<string> keys;
    private string jsonstring;
    private void Start()
    {
        foreach (string scene in scenes)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
        Invoke("CreateTemplate", 1);
    }

    private void CreateTemplate()
    {
        jsonstring = "{\n";
        foreach (TextPackTMPClient text in FindObjectsByType<TextPackTMPClient>(FindObjectsSortMode.None))
        {
            if (!keys.Contains(text.Key))
            {
                keys.Add(text.Key);
                jsonstring += $"    \"{text.Key}\": \"{text.Key}\",\n";
            }
        }
        foreach (string text in ExplicitKeys)
        {
            if (!keys.Contains(text))
            {
                keys.Add(text);
                jsonstring += $"    \"{text}\": \"{text}\",\n";
            }
        }
        jsonstring += "}";
        string filePath = Path.Combine(Application.dataPath, "Template.json");
        File.WriteAllText(filePath, jsonstring);
        Debug.LogWarning("JSON file saved to: " + filePath);
    }
}
