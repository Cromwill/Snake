using System.Collections;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class HapticsHelper : MonoBehaviour
{
    [SerializeField] private SnakeInitializer _initializer;

    private Head _snakeHead;

    private void OnEnable()
    {
        _initializer = FindObjectOfType<SnakeInitializer>();
        _initializer.Initialized += OnSnakeInitializer;
    }

    private void OnDisable()
    {
#if UNITY_IOS
        MMNViOS.iOSReleaseHaptics();
#endif

        _initializer.Initialized -= OnSnakeInitializer;
        if (_snakeHead != null)
        {
            _snakeHead.FoodFinded -= OnFoodFinded;
            _snakeHead.ObstacleEntered -= OnObstacleEntered;
        }
    }

    private void Awake()
    {
#if UNITY_IOS
        MMNViOS.iOSInitializeHaptics();
#endif
    }

    private void OnSnakeInitializer(Snake snake)
    {
        _snakeHead = snake.GetComponentInChildren<Head>();
        _snakeHead.FoodFinded += OnFoodFinded;
        _snakeHead.ObstacleEntered += OnObstacleEntered;
    }

    private void OnFoodFinded(Food food)
    {
        Debug.Log("Food finded");
        MMVibrationManager.Haptic(HapticTypes.SoftImpact);
    }

    private void OnObstacleEntered(Obstacle obstacle)
    {
        Debug.Log("Obstacle Intered");
        MMVibrationManager.Haptic(HapticTypes.Warning);
    }
}
