using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MobileInput : BaseInput
{
    protected override event UnityAction Touched;
    protected override event UnityAction Released;

    private void Update()
    {
#if UNITY_EDITOR

#else
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            if (Input.touchCount > 0)
            {
                var firstTouch = Input.GetTouch(Input.touchCount - 1);
                if (firstTouch.phase == TouchPhase.Began)
                    Touched?.Invoke();
                else if (firstTouch.phase == TouchPhase.Ended)
                    Released?.Invoke();

            }
        }
#endif
    }
}
