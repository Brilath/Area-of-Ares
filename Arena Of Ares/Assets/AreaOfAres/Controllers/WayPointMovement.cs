using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointMovement : MonoBehaviour
{

    [SerializeField] private GameObject _wayPointsGO;
    [SerializeField] private float _wayPointPadding;
    [SerializeField] private float _speed;
    [SerializeField] private float _numberOfWayPoints;
    [SerializeField] private int _currentWayPoint;
    private List<GameObject> _wayPoints;

    private void Awake()
    {
        _wayPoints = new List<GameObject>();
        foreach (Transform go in _wayPointsGO.transform)
        {
            _wayPoints.Add(go.gameObject);
        }
        _numberOfWayPoints = _wayPointsGO.transform.childCount;
        _currentWayPoint = 0;
    }

    private void Start()
    {
        if (_wayPoints.Count > 0)
        {
            transform.position = _wayPoints[0].transform.position;
        }
    }

    private void Update()
    {
        Vector2 targetWP = _wayPoints[_currentWayPoint].transform.position;

        transform.position = Vector2.Lerp(transform.position, targetWP, Time.deltaTime * _speed);

        if (Vector2.Distance(transform.position, targetWP) <= _wayPointPadding)
        {
            _currentWayPoint = NextWayPoint(_currentWayPoint, _wayPoints.Count - 1);
        }
    }

    private int NextWayPoint(int current, int max)
    {
        int nextWayPoint = current;
        if (nextWayPoint == max)
        {
            nextWayPoint = 0;
        }
        else
        {
            nextWayPoint++;
        }
        return nextWayPoint;
    }
}
