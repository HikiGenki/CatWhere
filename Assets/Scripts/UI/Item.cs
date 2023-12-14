using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;

    public Button Button { get { return button; } }

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(HandleButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on GameObject: " + gameObject.name);
        }
    }

    private void HandleButtonClick()
    {
        GameManager.Instance.ClickedOnItem(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HUD.Instance.DisplayHoverItemName(gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit!");
    }
}