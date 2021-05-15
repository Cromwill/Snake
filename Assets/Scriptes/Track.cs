using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EasyRoads3Dv3;

public class Track : MonoBehaviour
{
    [SerializeField] private ERModularRoad _roadModular;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _step;

    private Vector3[][] _tracks;
    private float _distanceTraveleds;
    private bool _isDrawing;

    public float DistanceLength => _roadModular.totalDistance;
    public float PlayerDistanceTraveleds { get; private set; }

    void Start()
    {
        Vector3[] firstLine = _roadModular.soSplinePoints.ToArray();
        Vector3[] secondLine = new Vector3[firstLine.Length];
        Vector3[] thirdtLine = new Vector3[firstLine.Length];

        Vector3[] leftPoints = _roadModular.soSplinePointsLeft.ToArray();
        Vector3[] rightPoints = _roadModular.soSplinePointsRight.ToArray();

        for (int i = 0; i < firstLine.Length; i++)
        {
            secondLine[i] = Vector3.Lerp(firstLine[i], leftPoints[i], _step);
            thirdtLine[i] = Vector3.Lerp(firstLine[i], rightPoints[i], _step);
        }

        _tracks = new Vector3[3][]
        {
            CreateLine(firstLine),
            CreateLine(secondLine),
            CreateLine(thirdtLine)
        };

        _isDrawing = true;
    }

    private Vector3[] CreateLine(Vector3[] positions)
    {
        List<Vector3> line = positions.ToList();
        float length = 0.0f;

        for (int i = 0; i < line.Count - 1; i++)
        {
            length += Vector3.Distance(line[i], line[i + 1]);
        }

        float step = length / (float)line.Count;

        for (int i = 0; i < line.Count - 1; i++)
        {
            float distance = Vector3.Distance(line[i], line[i + 1]);

            if (distance > step && i != line.Count - 2)
            {
                Vector3 position = Vector3.Lerp(line[i], line[i + 1], step / distance);
                line[i + 1] = position;
            }
            else if (distance < step && i != line.Count - 2)
            {
                float postStep = step - distance;
                distance = Vector3.Distance(line[i + 1], line[i + 2]);

                Vector3 position = Vector3.Lerp(line[i + 1], line[i + 2], postStep / distance);
                line[i + 1] = position;
            }

            if(i == line.Count - 2)
            {
                if(distance > step)
                {
                    Vector3 position = Vector3.Lerp(line[i], line[i + 1], step / distance);
                    line.Add(line[i + 1]);
                    line[i + 1] = position;
                }
                else if(distance < step)
                {
                    line.Remove(line[i + 1]);
                }
            }
        }
        return line.ToArray();
    }

    private void OnDrawGizmos()
    {
        if(_isDrawing)
        {
            Debug.Log("FerstOisition - " + GetPosition(0));
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetPosition(0), 0.5f);

            Debug.Log("lastPosition - " + GetPosition(1));
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(GetPosition(1), 0.5f);
        }
    }

    public Vector3 GetPosition(float length)
    {
        return GetPosition(length, 0);
    }

    public Vector3 GetPositionByIndex(float length, int index)
    {
        if (index < _tracks.Length)
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

        Vector3[] track = _tracks[index];

        float lengthStep = 1.0f / (track.Length - 1.0f);
        Vector3 position = Vector3.zero;


        for (int i = 0; i < track.Length - 1; i++)
        {
            float minLength = i * lengthStep;
            float maxLength = (i + 1) * lengthStep;


            if (minLength <= length && maxLength >= length)
            {
                if(length == 1)
                { }

                float leadedLength = 1 / ((i + 1) * lengthStep - i * lengthStep);
                float leadedPosition = (length - i * lengthStep) * leadedLength;
                position = Vector3.Lerp(track[i], track[i + 1], leadedPosition);
                position.y += 0.25f;
            }
        }

        return position;
    }

}