using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DangerLineDetected : MonoBehaviour
{
    [SerializeField] private Sprite _redLine;
    [SerializeField] private Sprite _greenLine;
    [SerializeField] private ObstacleExitTrigger _trigger;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _redLine;
        _trigger.TriggerExit += ToggleSprite;
    }

    private void ToggleSprite()
    {
        _spriteRenderer.sprite = _greenLine;
    }
}
