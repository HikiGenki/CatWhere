using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField]
    private List<Room> rooms;

    [SerializeField]
    private float fadeSpeed = 1f;

    private HUD hud;

    private Room activeRoom;
    private int activeRoomIndex = -1;

    private bool inTransition;
    private Action onComplete;

    public int TotalRoomsCount { get { return rooms.Count; } }
    public Item ActiveItem { get { return activeRoom.ActiveItem; } }

    private void Awake()
    {
        foreach (var room in rooms)
        {
            room.Init();
        }
    }

    private void Start()
    {
        hud = HUD.Instance;    
    }

    public void ShowRoom(int index, Action onComplete)
    {
        if (!inTransition && index != activeRoomIndex)
        {
            this.onComplete = onComplete;
            activeRoomIndex = index;
            index = index % TotalRoomsCount;
            StartCoroutine(RoomTransition(index));
        }
    }

    private IEnumerator RoomTransition(int index)
    {
        //Hide active room
        if (activeRoom != null)
        {
            yield return UIFadeUtil.FadeOutcanvasToTransparent(activeRoom.bg, fadeSpeed);
            yield return UIFadeUtil.FadeOutcanvasToTransparent(activeRoom.itemsRoot, fadeSpeed);
        }

        activeRoom = rooms[index];

        yield return UIFadeUtil.FadeInCanvasToOpaque(activeRoom.itemsRoot, fadeSpeed);
        yield return UIFadeUtil.FadeInCanvasToOpaque(activeRoom.bg, fadeSpeed);
        hud.RevealTimerGroup();

        inTransition = false;

        onComplete?.Invoke();
    }

    public void FoundItem() => activeRoom.FoundItemItem();

    public bool ShowNewItem() => activeRoom.TryShowNewItem();
}