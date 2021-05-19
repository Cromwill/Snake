using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatedObstacle : Obstacle
{
    private Animator _animator;

    protected override void OnAwake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void OnDisableObstacle()
    {
        _animator.SetBool("IsWorking", false);
    }

    protected override void OnEnableObstacle()
    {
        _animator.SetBool("IsWorking", true);
    }
 
}
