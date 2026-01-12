using TMPro;
using UnityEngine;

namespace MTPS
{
    public class TextPackTMPClient : MonoBehaviour
    {
        private TextPackDirector Extracter;
        private TMP_Text ClientText;
        public string Key;

        private void Start()
        {
            ClientText = GetComponent<TMP_Text>();
            Extracter = FindFirstObjectByType<TextPackDirector>();
            Key = ClientText.text;
            UpdateText();
        }

        public void UpdateText() => ClientText.text = Extracter.FCurrentTextPack[Key];
    }
}
