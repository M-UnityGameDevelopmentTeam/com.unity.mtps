using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextPackList : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject ButtonPrefab;
    private GameObject TempButton;
    private int tempindex = -1;
    private void Start()
    {
        foreach (TextPack textPack in FindFirstObjectByType<TextPackDirector>().FTextPacks)
        {
            tempindex += 1;
            TempButton = Instantiate(ButtonPrefab, Panel.transform);
            TempButton.GetComponentInChildren<TMP_Text>().text = textPack.Name;
            var a = tempindex;
            TempButton.GetComponent<Button>().onClick.AddListener(() => FindFirstObjectByType<TextPackDirector>().ChangeTextPack(a));
        }
    }
}
