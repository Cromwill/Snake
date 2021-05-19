using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObstacleDamageComponent : MonoBehaviour
{
    private Obstacle _parentObstacle;

    public Obstacle ParentObstacle => _parentObstacle;

    private void Awake()
    {
        _parentObstacle = GetComponentInParent<Obstacle>();
    }
}
