using UnityEngine;
using EasyRoads3Dv3;

public class Track : MonoBehaviour
{
    [SerializeField] private ERModularRoad _roadModular;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _step;

    private ERRoad _road;
    private LineRenderer _playerTrack;
    private float _distanceTraveled;

    void Start()
    {
        _road = new ERRoad(_roadModular);
        _distanceTraveled = 0;

        Vector3[] firstLine = _roadModular.splinePoints.ToArray();
        Vector3[] secondLine = new Vector3[firstLine.Length];
        Vector3[] thirdtLine = new Vector3[firstLine.Length];

        Vector3[] leftPoints = _roadModular.soSplinePointsLeft.ToArray();
        Vector3[] rightPoints = _roadModular.soSplinePointsRight.ToArray();

        for (int i = 0; i < firstLine.Length; i++)
        {
            secondLine[i] = Vector3.Lerp(firstLine[i], leftPoints[i], _step);
            thirdtLine[i] = Vector3.Lerp(firstLine[i], rightPoints[i], _step);
        }

        _playerTrack = CreateLine(firstLine, _line);
        CreateLine(secondLine, _line);
        CreateLine(thirdtLine, _line);
    }

    private void Update()
    {
        
    }

    private LineRenderer CreateLine(Vector3[] position, LineRenderer lineRenderer)
    {
        LineRenderer line = LineRenderer.Instantiate(lineRenderer);
        line.useWorldSpace = true;
        line.positionCount = position.Length;
        line.SetPositions(position);
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;

        return line;
    }

    public Vector3 GetPosition(float length)
    {
        Vector3[] track = new Vector3[_playerTrack.positionCount];
        _playerTrack.GetPositions(track);

        float lengthStep = 1.0f / track.Length;
        Vector3 position = Vector3.zero;
        _distanceTraveled += length;

        for(int i = 0; i < track.Length - 1; i++)
        {
            if((i * lengthStep) <= _distanceTraveled && ((i+1) * lengthStep) > _distanceTraveled)
            {
                float leadedLength = 1 / ((i + 1) * lengthStep - i * lengthStep);
                float leadedPosition = (_distanceTraveled - i * lengthStep) * leadedLength;
                position = Vector3.Lerp(track[i], track[i + 1], leadedPosition);
                position.y += 0.25f;
            }
        }

        return position;
    }
}

public class TrackData
{
    private LineRenderer _lineRenderer;
    private Vector3[] _positions;

    public TrackData(LineRenderer lineRenderer)
    {
        _lineRenderer = lineRenderer;
        _positions = new Vector3[_lineRenderer.positionCount];
        _lineRenderer.GetPositions(_positions);
    }




}
