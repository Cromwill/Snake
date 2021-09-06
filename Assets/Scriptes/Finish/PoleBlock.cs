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

    private Coroutine _pingPongColorCoroutine;

    private void Awake()
    {
        _selfMaterial = GetComponent<Renderer>().material;
        _numberMaterial = _numberRenderer.material;

        _pingPongColorCoroutine = null;
    }

    public void Init(Color blockColor, Texture blockTexture, int giftValue)
    {
        _selfMaterial.color = blockColor;
        _numberMaterial.mainTexture = blockTexture;
        GiftValue = giftValue;
    }

    public void LightUp()
    {
        _selfMaterial.SetFloat("_Emission", 0.25f);
        _numberMaterial.color = Color.white;
    }

    public void LightDown()
    {
        _selfMaterial.SetFloat("_Emission", 0f);
        _numberMaterial.color = Color.gray;
    }

    public void PingPongColor(Color secondColor)
    {
        if (_pingPongColorCoroutine != null)
            StopCoroutine(_pingPongColorCoroutine);

        _pingPongColorCoroutine = StartCoroutine(PingPongColorCoroutine(_selfMaterial.color, secondColor));
    }

    private IEnumerator PingPongColorCoroutine(Color firstColor, Color secondColor)
    {
        var targetColor = secondColor;

        while (true)
        {
            _selfMaterial.color = Color.LerpUnclamped(_selfMaterial.color, targetColor, 10f * Time.deltaTime);

            if (_selfMaterial.color == targetColor)
            {
                if (targetColor == secondColor)
                    targetColor = firstColor;
                else
                    targetColor = secondColor;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
