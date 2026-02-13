using TMPro;
using UnityEngine;

namespace MTPS
{
    public class TextPackTMPClient : MonoBehaviour, ITextPackClient
    {
        private TextPackDirector textDirector;
        private TMP_Text clientText;
        private string Key;

        public string[] GetKeys() => new string[] { Key };

        private void Awake()
        {
            clientText = GetComponent<TMP_Text>();
            Key = clientText.text;
            textDirector = FindFirstObjectByType<TextPackDirector>();
        }

        public void TextPackUpdate()
        {
            clientText.text = textDirector.GetKeyFromTextPack(Key);
        }
    }
}
