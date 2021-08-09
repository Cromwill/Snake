using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PagePoint : MonoBehaviour
{
    private const string SelectedKey = "Selected";

    private Animator _selfAnimator;

    private void Awake()
    {
        _selfAnimator = GetComponent<Animator>();
    }

    public void Select()
    {
        _selfAnimator.SetBool(SelectedKey, true);
    }

    public void Deselect()
    {
        _selfAnimator.SetBool(SelectedKey, false);
    }
}
