using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpriteRendererButton : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color hoverColor = Color.grey;
    public Color clickColor = Color.red;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        Debug.Log("hi");
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }
    }

    private void OnMouseDown()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = clickColor;
        }
    }

    private void OnMouseUp()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hoverColor;
        }

        Debug.Log("Button clicked!");
    }
}
