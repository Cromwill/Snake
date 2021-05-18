using System.Collections;
using UnityEngine;

public class Propeller : ObstacleOld
{
    private Animator _selfAnimator;

    private void Start()
    {
        _selfAnimator = GetComponent<Animator>();
        ObstacleExitTrigger trigger = GetComponentInChildren<ObstacleExitTrigger>();

        trigger.TriggerExit += EnableObstacle;

    }

    private void EnableObstacle()
    {
        _selfAnimator.SetBool("IsWorked", false);
        ToggleSignal();
    }

}