using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHanlder = null;

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHanlder != null) OnDragHanlder.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(OnClickHandler != null) OnClickHandler.Invoke(eventData);
    }
}

