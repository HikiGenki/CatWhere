using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIPanningMenuBG : MonoBehaviour
{    
    [SerializeField]
    private float xSpeed = -10;

    private float minX = -1920;
    private float xOffset = 1920;
    private Image image;

    private void Start ()
    {
        image = GetComponent<Image>();
	}

    private void Update ()
    {
        Vector3 pos = image.rectTransform.position;
        pos.x += xSpeed * Time.deltaTime;

        if (pos.x < minX)
        {
            pos.x += xOffset * 2;
        }

        image.rectTransform.position = pos;
    }
}
