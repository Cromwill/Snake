using UnityEngine;

public class ObstacleDamager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _punchEffect;
    private ObstacleOld _parentObstacle;

    public ObstacleOld ParentObstacle 
    { 
        get
        {
            if(_parentObstacle == null)
                _parentObstacle = GetComponentInParent<ObstacleOld>();
            return _parentObstacle;
        }
    }

    private void Awake()
    {
        _parentObstacle = GetComponentInParent<ObstacleOld>();
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
