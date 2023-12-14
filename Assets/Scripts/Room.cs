using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public CanvasGroup bg;

    [Tooltip("The root game object containing the item objects")]
    public CanvasGroup itemsRoot;

    private List<Item> availableItems = new List<Item>();

    private Item activeItem;
    private int activeItemIndex;

    public Item ActiveItem => activeItem;

    public void Init()
    {
        Reset();
        availableItems.AddRange(itemsRoot.GetComponentsInChildren<Item>());
        UIFadeUtil.SetCanvasToTransparent(bg);
        UIFadeUtil.SetCanvasToTransparent(itemsRoot);
    }

    public bool TryShowNewItem()
    {
        if (availableItems.Count <= 0f)
        {
            Debug.LogError("No available items");
            return false;
        }

        activeItemIndex = Random.Range(0, availableItems.Count);
        activeItem = availableItems[activeItemIndex];
        return true;
    }

    public void FoundItemItem()
    {
        availableItems.RemoveAt(activeItemIndex);
    }

    public void Reset()
    {
        activeItem = null;
        activeItemIndex = -1;
    }
}