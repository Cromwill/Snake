using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MobileInput : BaseInput
{
    protected override event UnityAction Touched;
    protected override event UnityAction Released;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (IsPointerOverUIObject(Input.touches[0].position) == false)
            {
                var firstTouch = Input.GetTouch(Input.touchCount - 1);
                if (firstTouch.phase == TouchPhase.Began)
                    Touched?.Invoke();
                else if (firstTouch.phase == TouchPhase.Ended)
                    Released?.Invoke();
            }
        }
    }

    public bool IsPointerOverUIObject(Vector2 inputPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = inputPosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }

        return false;
    }
}
