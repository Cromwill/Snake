using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PoleBlock : MonoBehaviour
{
    [SerializeField] private Renderer _numberRenderer;

    private Material _selfMaterial;
    private Material _numberMaterial;

    public int GiftValue { get; private set; }

    private void Awake()
    {
        _selfMaterial = GetComponent<Renderer>().material;
        _numberMaterial = _numberRenderer.material;
    }

    public void Init(Color blockColor, Texture blockTexture, int giftValue)
    {
        _selfMaterial.color = blockColor;
        _numberMaterial.mainTexture = blockTexture;
        GiftValue = giftValue;
    }
}
