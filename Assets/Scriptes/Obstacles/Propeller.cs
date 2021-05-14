using System.Collections;
using UnityEngine;

public class Propeller : Obstacle
{
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

}