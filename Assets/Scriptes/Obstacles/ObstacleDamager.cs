using UnityEngine;

public class ObstacleDamager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _punchEffect;
    private Obstacle _parentObstacle;

    public Obstacle ParentObstacle 
    { 
        get
        {
            if(_parentObstacle == null)
                _parentObstacle = GetComponentInParent<Obstacle>();
            return _parentObstacle;
        }
    }

    private void Awake()
    {
        _parentObstacle = GetComponentInParent<Obstacle>();
    }

    public virtual void OnPlayerPunch()
    {
        if (_parentObstacle != null)
        {
            _parentObstacle.OnPlayerPunch();
            if (_punchEffect != null)
                Instantiate(_punchEffect, transform.position, Quaternion.identity);
        }
    }
}
