using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRenderer : MonoBehaviour
{
    private readonly Queue<Vector3> _positions = new Queue<Vector3>();
    private int lineCount = 10;

    private float _timer;

    [SerializeField] private float swipeLineLifetime;
    [SerializeField] private LineRenderer line;

    public void AddSwipeLine(Vector2 startPoint)
    {
        _positions.Clear();
        line.positionCount = 2;

        _positions.Enqueue(startPoint);
        _positions.Enqueue(startPoint);
        line.SetPositions(_positions.ToArray());
    }

    public void ContinueSwipeLine(Vector2 nextPosition)
    {
        if (_positions.Count >= lineCount)
        {
            _positions.Dequeue();
        }

        if (_positions.Count == 0)
        {
            AddSwipeLine(nextPosition);
        }
        else
        {
            _positions.Enqueue(nextPosition);

            line.positionCount = _positions.Count;
            line.SetPositions(_positions.ToArray());
        }
    }

    private void UpdateSwipeLine()
    {
        line.positionCount = _positions.Count;
        line.SetPositions(_positions.ToArray());
    }

    public void EndSwipeLine()
    {
        _positions.Clear();
        line.positionCount = 0;
    }

    void Update()
    {
        if (_positions.Count > 0)
        {
            if (_timer > swipeLineLifetime)
            {
                _positions.Dequeue();

                UpdateSwipeLine();

                _timer = 0;
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
        else
        {
            _timer = 0;
        }
    }

}
