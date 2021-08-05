using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BonusFinish : MonoBehaviour
{
    [Header("Pole references"), Space(5f)]
    [SerializeField] private BonusPole _leftPole;
    [SerializeField] private BonusPole _rightPole;
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

    public bool IsJumping { get; private set; }
    public float DistanceLength { get; private set; }

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
#endif

    #endregion

    private void Start()
    {
        _jumps = new Stack<JumpInfo>();

        _leftPole.Init(_angleDelta, _radiusScale);
        _rightPole.Init(_angleDelta, _radiusScale);

        DistanceLength = _poleScale.y * 3f;

        _firstPole = _rightPole;
        _currentPole = _firstPole;
        IsJumping = false;
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
