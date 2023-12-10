using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private Image bar;

    private void Awake()
    {
        Instance = this;
    }

    public void Reset()
    {
        bar.fillAmount = 1f;
    }

    public void SetName(string name)
    {
        itemName.text = name;
    }

    public void UpdateBar(float percentage)
    {
        bar.fillAmount = percentage;
    }
}
