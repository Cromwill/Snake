using UnityEngine;

public class ObstacleSignalSwitcher : MonoBehaviour
{
    [SerializeField] private Renderer[] _signalRenderers;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _greenMaterial;

    public void SetRed() => SetMaterial(_redMaterial);

    public void SetGreen() => SetMaterial(_greenMaterial);

    private void SetMaterial(Material material)
    {
        foreach (var signal in _signalRenderers)
            signal.material = material;
    }
}
