using TMPro;
using UnityEngine;

namespace MTPS
{
    public class TextPackTMPClient : MonoBehaviour
    {
        private TextPackDirector textDirector;
        private TMP_Text clientText;
        public string Key;

        private void Start()
        {
            clientText = GetComponent<TMP_Text>();
            textDirector = FindFirstObjectByType<TextPackDirector>();
            Key = clientText.text;
            UpdateText();
        }

        public void UpdateText()
        {
            clientText.text = textDirector.GetKeyFromTextPack(Key);
        }
    }
}
