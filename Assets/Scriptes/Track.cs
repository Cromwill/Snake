﻿using UnityEngine;
using EasyRoads3Dv3;

public class Track : MonoBehaviour
{
    [SerializeField] private ERModularRoad _roadModular;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _step;

    private LineRenderer[] _tracks;
    private float _distanceTraveleds;

    public float DistanceLength => _roadModular.totalDistance;
    public float PlayerDistanceTraveleds { get; private set; }

    void Start()
    {
        Vector3[] firstLine = _roadModular.soSplinePoints.ToArray();
        Vector3[] secondLine = new Vector3[firstLine.Length];
        Vector3[] thirdtLine = new Vector3[firstLine.Length];

        Vector3[] leftPoints = _roadModular.soSplinePointsLeft.ToArray();
        Vector3[] rightPoints = _roadModular.soSplinePointsRight.ToArray();

        for(int i = 0; i < firstLine.Length; i++)
        {
            firstLine[i].y += 0.1f;
            leftPoints[i].y += 0.1f;
            rightPoints[i].y += 0.1f;
        }

        for (int i = 0; i < firstLine.Length; i++)
        {
            secondLine[i] = Vector3.Lerp(firstLine[i], leftPoints[i], _step);
            thirdtLine[i] = Vector3.Lerp(firstLine[i], rightPoints[i], _step);
        }

        _tracks = new LineRenderer[3]
        {
            CreateLine(firstLine, _line),
            CreateLine(secondLine, _line),
            CreateLine(thirdtLine, _line)
        };
    }

    private LineRenderer CreateLine(Vector3[] position, LineRenderer lineRenderer)
    {
        LineRenderer line = Instantiate(lineRenderer);
        line.useWorldSpace = true;
        line.positionCount = position.Length;
        line.SetPositions(position);
        line.startColor = new Color(0, 0, 0, 0);
        line.endColor = new Color(0, 0, 0, 0);
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;

        return line;
    }

    public Vector3 GetPosition(float length)
    {
        return GetPosition(length, 0);
    }

    public Vector3 GetPositionByIndex(float length, int index)
    {
        if(index < _tracks.Length)
        {
            return GetPosition(length, index);
        }
        else
        {
            Debug.LogError("the index is out of bounds of the array");
            return Vector3.zero;
        }
    }

    private Vector3 GetPosition(float length, int index)
    {
        if (index == 0)
            PlayerDistanceTraveleds = length;

        Vector3[] track = new Vector3[_tracks[index].positionCount];
        _tracks[index].GetPositions(track);

        float lengthStep = 1.0f / track.Length;
        Vector3 position = Vector3.zero;

        for (int i = 0; i < track.Length - 1; i++)
        {
            if ((i * lengthStep) <= length && ((i + 1) * lengthStep) > length)
            {
                float leadedLength = 1 / ((i + 1) * lengthStep - i * lengthStep);
                float leadedPosition = (length - i * lengthStep) * leadedLength;
                position = Vector3.Lerp(track[i], track[i + 1], leadedPosition);
                position.y += 0.25f;
            }
        }

        return position;
    }
}