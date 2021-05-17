using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pole : MonoBehaviour
{
    [SerializeField] private Snake _playerSnake;
    [SerializeField] private float _angleDelta;
    [SerializeField] private PoleBlock _blockTemplate;
    [SerializeField] private Texture[] _textures;

    public event UnityAction<int> SnakeCrawled;

    public float DistanceLength { get; private set; }

    private List<PoleBlock> _blocks;
    private SnakeBoneMovement _snakeBoneMovement;
    private Head _snakeHead;

    private void OnValidate()
    {
        if (_angleDelta <= 0)
            _angleDelta = 1f;
    }

    private void Awake()
    {
        _snakeBoneMovement = _playerSnake.GetComponent<SnakeBoneMovement>();
        _snakeHead = _playerSnake.GetComponentInChildren<Head>();
    }

    private void Start()
    {
        DistanceLength = transform.lossyScale.y * _angleDelta;
        SpawnBlocks();
    }

    private void OnEnable()
    {
        _snakeBoneMovement.Partially—rawled += OnsnakePartiallyCrawled;
        _snakeBoneMovement.Full—rawled += OnSnakeFullCrawled;
    }

    private void OnDisable()
    {
        _snakeBoneMovement.Partially—rawled -= OnsnakePartiallyCrawled;
        _snakeBoneMovement.Full—rawled -= OnSnakeFullCrawled;
    }

    private void OnSnakeFullCrawled()
    {
        var lastBlock = _blocks[_blocks.Count - 1];
        SnakeCrawled?.Invoke(lastBlock.GiftValue);
    }

    private void OnsnakePartiallyCrawled(float distance)
    {
        var nearestBlock = GetNearestBlock(_snakeHead.transform.position);
        nearestBlock.PingPongColor(Color.white);

        SnakeCrawled?.Invoke(nearestBlock.GiftValue);
    }

    public Vector3 GetPositionByParameter(float t)
    {
        float deltaRad = 2 * Mathf.PI * t * _angleDelta;

        var posHeight = transform.position + Vector3.down * transform.lossyScale.y + Vector3.up * 2 * transform.lossyScale.y * t;

        posHeight += transform.forward * Mathf.Cos(deltaRad) * transform.lossyScale.z / 2f;
        posHeight += transform.right * Mathf.Sin(deltaRad) * transform.lossyScale.x / 2f;

        return posHeight;
    }

    private void SpawnBlocks()
    {
        _blocks = new List<PoleBlock>();

        var blockCount = _textures.Length;
        var blockHeight = transform.lossyScale.y * 2f / blockCount;
        var blockPosition = transform.position - transform.up * transform.lossyScale.y + transform.up * blockHeight / 2f;

        var startColor = new Color(255f / 255f, 127f / 255f, 0);
        var endColor = new Color(148f / 255f, 0, 211f / 255f);

        for (int i = 0; i < blockCount; i++)
        {
            var block = Instantiate(_blockTemplate, blockPosition, Quaternion.identity, transform);
            var color = Color.LerpUnclamped(startColor, endColor, i / (float)blockCount);

            block.transform.localScale = new Vector3(1f, blockHeight / transform.lossyScale.y / 2f, 1f);
            block.Init(color, _textures[i], i * 10);

            blockPosition += transform.up * blockHeight;

            _blocks.Add(block);
        }
    }

    private PoleBlock GetNearestBlock(Vector3 fromPosition)
    {
        var minDistance = float.MaxValue;
        var nearestBlock = _blocks[0];

        foreach (var block in _blocks)
        {
            var distance = Vector3.Distance(fromPosition, block.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBlock = block;
            }
        }

        return nearestBlock;
    }
}
