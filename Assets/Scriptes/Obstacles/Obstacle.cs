using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _obstacleSignals;
    [SerializeField] private bool _isPuncher;
    [SerializeField] private Material _workingMaterial;
    [SerializeField] private Material _notWorkingMaterial;
    
    private bool _isDamageable;

    public bool IsPuncher => _isPuncher;
    public bool IsDamageable => _isDamageable;


    private Animator _selfAnimator;

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();
        ObstacleTrigger trigger = GetComponentInChildren<ObstacleTrigger>();

        trigger.TriggerExit += EnableObstacle;
    }

    private void EnableObstacle()
    {
        _selfAnimator.SetBool("IsWorked", false);
        ToggleSignal();
    }

    public void OnPlayerPunch()
    {
        EnableObstacle();
    }

    public void ToggleSignal()
    {
        if (_obstacleSignals != null && _obstacleSignals.Length > 0)
        {
            foreach (var signal in _obstacleSignals)
                signal.material = _notWorkingMaterial;
        }
    }

    public void EnableDamageable()
    {
        _isDamageable = true;
    }

    public void DisableDamageable()
    {
        _isDamageable = false;
    }
}
