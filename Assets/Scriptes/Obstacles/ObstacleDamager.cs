using UnityEngine;

public class ObstacleDamager : MonoBehaviour
{
    [SerializeField] private Material _dangerous;
    [SerializeField] private Material _safe;

    private Renderer _selfRenderer;

    private void Awake()
    {
        _selfRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (transform.localScale.y == 0.05f)
            _selfRenderer.material = _safe;
        else
            _selfRenderer.material = _dangerous;
    }
}
