using TMPro;
using UnityEngine;

namespace MTPS
{
    public class TextPackTMPClient : MonoBehaviour, ITextPackClient
    {
        private TextPackDirector textDirector;
        private TMP_Text clientText;
        private string Key;

        public string[] GetKeys()
        {
            clientText = GetComponent<TMP_Text>();
            Key = clientText.text;
            return new string[] { Key };
        }

        private void Awake()
        {
            textDirector = FindFirstObjectByType<TextPackDirector>();
            TextPackUpdate();
        }

        public void TextPackUpdate() => clientText.text = textDirector.GetKeyFromTextPack(Key);
    }
}
