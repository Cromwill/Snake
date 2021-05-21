using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatedObstacle : Obstacle
{
    [SerializeField] private string _disableClipName;

    private Animator _animator;

    protected override void OnAwake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void OnDisableObstacle()
    {
        _animator.SetBool("IsWorking", false);
        _animator.Play(_disableClipName);
    }

    protected override void OnEnableObstacle()
    {
        _animator.SetBool("IsWorking", true);
    }
 
}
