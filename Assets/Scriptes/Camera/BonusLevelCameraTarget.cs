using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevelCameraTarget : MonoBehaviour
{
    [SerializeField] private SnakeInitializer _snakeInitializer;
    [SerializeField] private BonusFinish _bonusFinish;

    private Snake _followingSnake;

    private void OnEnable()
    {
        _snakeInitializer.Initialized += OnSnakeInitialized;
    }

    private void OnDisable()
    {
        _snakeInitializer.Initialized -= OnSnakeInitialized;
    }

    private void OnSnakeInitialized(Snake snake)
    {
        _followingSnake = snake;
        transform.position = _bonusFinish.transform.position;
    }

    private void Update()
    {
        var nextPosition = new Vector3(transform.position.x, _followingSnake.HeadTransform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, nextPosition, 3f * Time.deltaTime);
    }
}
