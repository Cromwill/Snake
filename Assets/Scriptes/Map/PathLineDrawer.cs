using System;
using UnityEngine;

[RequireComponent(typeof(Track))]
public class PathLineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer _pathLinePrefab;
    [SerializeField] private LineRenderer _coveredLinePrefab;

    private Track _track;
    private LineRenderer _pathLine;
    private LineRenderer _coveredLine;

    void Start()
    {
        _track = GetComponent<Track>();

        _pathLine = Instantiate(_pathLinePrefab, Vector3.zero, Quaternion.identity);
        _coveredLine = Instantiate(_coveredLinePrefab, Vector3.zero, Quaternion.identity);

        SetLineWidth(_pathLine, 0.5f);
        SetLineWidth(_coveredLine, 1.0f);

        DrawLine(0);
    }

    public void DrawLine(float coveredDistance)
    {
        SetPositionsOnLineRenderer(_pathLine, GetPositions(_track.DistanceLength, coveredDistance - 1.5f));
        SetPositionsOnLineRenderer(_coveredLine, GetPositions(0, coveredDistance + 1.65f));
    }

    private Vector3[] GetPositions(float startLine, float finishLine)
    {
        int count = Mathf.FloorToInt(Mathf.Abs(finishLine - startLine));
        Vector3[] positions = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            float distance = startLine < finishLine ? startLine + i : startLine - i;

            if (distance <= 0)
                distance = 0.1f;
            if (distance > _track.DistanceLength)
                distance = _track.DistanceLength - 0.1f;

            positions[i] = _track.GetPositionByDistance(distance);
            positions[i].y = -0.2f;
        }

        return positions;
    }

    private void SetPositionsOnLineRenderer(LineRenderer line, Vector3[] positions)
    {
        line.positionCount = positions.Length;
        line.SetPositions(positions);
    }

    private void SetLineWidth(LineRenderer line, float width)
    {
        line.startWidth = width;
        line.endWidth = width;
    }
}