using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class BonusFinish : MonoBehaviour
{
    [Header("Cup")]
    [SerializeField] private GameObject _cupObject;
    [Header("Pole references"), Space(5f)]
    [SerializeField] private BonusPole _leftPole;
    [SerializeField] private BonusPole _rightPole;
    [SerializeField] private Material _poleMaterial;
    [Header("Pole parameters"), Space(5f)]
    [SerializeField] private Vector3 _poleScale;
    [SerializeField] private float _distanceBetweenPoles;
    [Header("Move parameters"), Space(5f)]
    [SerializeField] private float _angleDelta;
    [SerializeField] private float _radiusScale;
    [Range(0, 0.2f), SerializeField] private float _jumpHeight;

    private Stack<JumpInfo> _jumps;
    private BonusPole _firstPole;
    private BonusPole _currentPole;
    private SnakeBoneMovement _snakeBoneMovement;

    public float DistanceLength { get; private set; }

    public event UnityAction Finished;

    #region EditorMethods

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_leftPole)
            _leftPole.transform.localScale = _poleScale;
        if (_rightPole)
            _rightPole.transform.localScale = _poleScale;

        if (_leftPole && _rightPole)
        {
            _leftPole.transform.localPosition = Vector3.left * _distanceBetweenPoles;
            _rightPole.transform.localPosition = Vector3.right * _distanceBetweenPoles;
        }
    }

    private void OnValidate()
    {
        DistanceLength = _poleScale.y * 3f;

        if (_leftPole)
            _leftPole.Init(_angleDelta, _radiusScale, DistanceLength);
        if (_rightPole)
            _rightPole.Init(_angleDelta, _radiusScale, DistanceLength);

        if (_poleMaterial)
            _poleMaterial.SetFloat("_TopLine", _poleScale.y * 2f);
    }

#endif

    #endregion

    private void OnDisable()
    {
        if (_snakeBoneMovement)
            _snakeBoneMovement.BonusPole—rawled -= OnSnakeCrawled;
    }


    private void Start()
    {
        _jumps = new Stack<JumpInfo>();

        DistanceLength = _poleScale.y * 3f;

        _leftPole.Init(_angleDelta, _radiusScale, DistanceLength);
        _rightPole.Init(_angleDelta, _radiusScale, DistanceLength);


        _firstPole = _rightPole;
        _currentPole = _firstPole;
    }

    public void Init(SnakeBoneMovement snakeBoneMovement)
    {
        _snakeBoneMovement = snakeBoneMovement;
        _snakeBoneMovement.BonusPole—rawled += OnSnakeCrawled;
    }

    public Vector3 GetPositionByDistance(float distance)
    {
        return GetPositionByParameter(distance / DistanceLength);
    }

    public Vector3 GetPositionByParameter(float t)
    {
        var state = GetStateByParameter(t);

        if (state == PositionState.LeftPole)
            return _leftPole.GetPositionByParameter(t);
        if (state == PositionState.RightPole)
            return _rightPole.GetPositionByParameter(t);

        var jump = GetJumpByParameter(t);
        var toPosition = jump.To.GetPositionByParameter(jump.EndJumpParameter);
        var fromPosition = jump.From.GetPositionByParameter(jump.StartJumpParameter);
        var direction = toPosition - fromPosition;
        var distance = (t - jump.StartJumpParameter) / (jump.EndJumpParameter - jump.StartJumpParameter);

        return fromPosition + direction * distance;
    }

    public void Jump(float t)
    {
        if (t > 1f - _jumpHeight)
            return;

        if (_jumps.Count != 0)
            if (_jumps.Peek().EndJumpParameter > t)
                return;

        var nextPole = GetNextPole(_currentPole);

        _jumps.Push(new JumpInfo(_currentPole, nextPole, t, t + _jumpHeight));
        _currentPole = nextPole;
    }

    private BonusPole GetNextPole(BonusPole pole)
    {
        return pole.Equals(_leftPole) ? _rightPole : _leftPole;
    }

    private void OnSnakeCrawled()
    {
        //var topPosition = _currentPole.transform.position + _currentPole.transform.up * _currentPole.transform.lossyScale.y;
        //_cupObject.transform.position = topPosition;
        _cupObject.SetActive(true);

        Finished?.Invoke();
    }

    private JumpInfo GetJumpByParameter(float t)
    {
        foreach (var jump in _jumps)
        {
            if (t > jump.StartJumpParameter && t < jump.EndJumpParameter)
                return jump;
        }

        return default;
    }

    private PositionState GetStateByParameter(float t)
    {
        var state = _firstPole.Equals(_leftPole) ? PositionState.LeftPole : PositionState.RightPole;

        foreach (var jump in _jumps)
        {
            if (jump.StartJumpParameter < t)
                state = GetNextState(state);
            if (jump.EndJumpParameter < t)
                state = GetNextState(state);
        }

        return state;
    }

    private PositionState GetNextState(PositionState state)
    {
        return (PositionState)(((int)state + 1) % 4);
    }

    public enum PositionState
    {
        RightPole = 0, LeftJumping, LeftPole, RightJumping
    }

    public struct JumpInfo
    {
        private BonusPole _from;
        private BonusPole _to;
        private float _startJumpParameter;
        private float _endJumpParameter;

        public BonusPole From => _from;
        public BonusPole To => _to;
        public float StartJumpParameter => _startJumpParameter;
        public float EndJumpParameter => _endJumpParameter;

        public JumpInfo(BonusPole from, BonusPole to, float startJumpParameter, float endJumpParameter)
        {
            _from = from;
            _to = to;
            _startJumpParameter = startJumpParameter;
            _endJumpParameter = endJumpParameter;
        }
    }
}
