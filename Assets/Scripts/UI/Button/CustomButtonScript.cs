using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;

    private void Start()
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
        Debug.Log("Button Clicked!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Over!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit!");
    }
}