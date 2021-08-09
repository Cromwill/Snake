using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HatData : GUIDData
{
    [SerializeField] private SnakeHat _hatTemplate;
    [SerializeField] private Sprite _hatPreview;

    public SnakeHat Prefab => _hatTemplate;
    public Sprite HatPreview => _hatPreview;
}
