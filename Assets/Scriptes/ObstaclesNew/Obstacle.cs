using UnityEngine;

[RequireComponent(typeof(ObstacleSignalSwitcher))]
public abstract class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleExitTrigger _obstacleExitTrigger;
    
    private ObstacleSignalSwitcher _signalSwither;

    private void Awake()
    {
        _signalSwither = GetComponent<ObstacleSignalSwitcher>();

        OnAwake();
    }

    private void OnEnable()
    {
        _obstacleExitTrigger.TriggerExit += OnObstacleTriggerExit;
    }

    private void OnDisable()
    {
        _obstacleExitTrigger.TriggerExit -= OnObstacleTriggerExit;
    }

    private void OnObstacleTriggerExit()
    {
        DisableObstacle();
    }

    public void EnableObstacle()
    {
        _signalSwither.SetRed();
        OnEnableObstacle();
    }

    public void DisableObstacle()
    {
        _signalSwither.SetGreen();
        OnDisableObstacle();
    }

    protected virtual void OnAwake() { }

    protected abstract void OnEnableObstacle();
    protected abstract void OnDisableObstacle();
}
