using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Food : MonoBehaviour
{
    [SerializeField] private ParticleSystem _eatingEffectTemplate;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Collider _collider;

    public Vector3 ColliderCenterPosition => _collider.bounds.center;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Eating()
    {
        Instantiate(_eatingEffectTemplate, transform.position + Vector3.up, transform.rotation);
        _meshRenderer.enabled = false;
    }
}
