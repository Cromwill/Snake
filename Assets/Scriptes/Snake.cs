using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class Snake : MonoBehaviour
{
    [SerializeField] private float _speedTime;
    [SerializeField, Range(1, 100)] private float _distanceBetweenSegments = 1;
    [SerializeField] private Track _track;
    [SerializeField] private Transform _target;

    private Spline _selfSpline;
    private float _distanceCovered;

    private void Start()
    {
        _distanceCovered = 0;
        _selfSpline = GetComponent<Spline>();
    }

    private void Update()
    {
        if (_distanceCovered <= 0.99f)
            Move();

        if (Input.GetKeyDown(KeyCode.Space))
            AddNodeInTail();
        if (Input.GetKeyDown(KeyCode.R))
            RemoveNodeFromTail();
    }

    private void Move()
    {
        _distanceCovered += 1 / _speedTime * Time.deltaTime;
        if (_target != null)
            _target.position = _track.GetPosition(_distanceCovered);

        for (int i = 0; i < _selfSpline.nodes.Count; i++)
        {
            var distance = _distanceCovered - i / (100 - _distanceBetweenSegments) / 10;
            if (distance < 0)
                continue;
            _selfSpline.nodes[i].Position = _track.GetPosition(distance) - transform.position;
        }
    }

    private void AddNodeInTail()
    {
        var tailNode = _selfSpline.nodes[_selfSpline.nodes.Count - 1];
        SplineNode newNode = new SplineNode(tailNode.Direction, tailNode.Direction + tailNode.Direction - tailNode.Position);
        _selfSpline.AddNode(newNode);
    }

    private void RemoveNodeFromTail()
    {
        if (_selfSpline.nodes.Count <= 2)
            return;

        var tailNode = _selfSpline.nodes[_selfSpline.nodes.Count - 1];
        _selfSpline.RemoveNode(tailNode);
    }
}
