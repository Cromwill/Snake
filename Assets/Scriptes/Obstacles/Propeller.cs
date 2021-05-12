using System.Collections;
using UnityEngine;

namespace Assets.Scriptes.Obstacles
{
    public class Propeller : Obstacle
    {
        private Animator _selfAnimator;

        // Use this for initialization
        void Start()
        {
            _selfAnimator = GetComponent<Animator>();
            ObstacleTrigger trigger = GetComponentInChildren<ObstacleTrigger>();

            trigger.TriggerExit += () =>
            {
                _selfAnimator.SetBool("IsWorked", false);
                ToggleSignal();
            };
        }

    }
}