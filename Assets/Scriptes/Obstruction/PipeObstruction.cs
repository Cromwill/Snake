using UnityEngine;

public class PipeObstruction : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private Transform _selfTransform;


    void Start()
    {
        _selfTransform = GetComponent<Transform>();
    }

    void Update()
    {
        _selfTransform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
}
