using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIPanningMenuBG : MonoBehaviour
{
    [SerializeField]
    private float maxPanSpeed = 2f;
    [SerializeField]
    private float acceleration = 2f;

    [Space]
    [SerializeField]
    private float maxX = 1920f;
    [SerializeField]
    private float minX = -1920f;

    private float currentPanSpeed;
    private bool panningRight;
    private RectTransform rectTransform;
    private Vector3 position;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        MoveUpdate();

        DirectionChangeUpdate();
    }

    private void MoveUpdate()
    {
        //Acceleration
        if (panningRight)
        {
            if (currentPanSpeed < maxPanSpeed)
            {
                currentPanSpeed += acceleration * Time.deltaTime;
            }
        }
        else
        {
            if (currentPanSpeed > -maxPanSpeed)
            {
                currentPanSpeed -= acceleration * Time.deltaTime;
            }
        }

        position = rectTransform.localPosition;
        position.x += currentPanSpeed * Time.deltaTime;
        rectTransform.localPosition = position;
    }

    private void DirectionChangeUpdate()
    {
        if (panningRight && rectTransform.localPosition.x > maxX)
        {
            panningRight = false;
        }
        if (!panningRight && rectTransform.localPosition.x < minX)
        {
            panningRight = true;
        }
    }
}
