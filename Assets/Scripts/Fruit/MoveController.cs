using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Vector2 Direction;

    [SerializeField] private Vector2 attractiveForce;
    [SerializeField] private float minRotateSpeed;
    [SerializeField] private float maxRotateSpeed;

    private float _randomRotateSpeed;

    private void Start()
    {
        _randomRotateSpeed = GetRandomRotateSpeed();
    }

    void FixedUpdate()
    {
        transform.Translate(Direction, Space.World);
        transform.Rotate(new Vector3(0, 0, _randomRotateSpeed));
        Direction += attractiveForce;
    }

    private float GetRandomRotateSpeed()
    {
        var direction = new int[] { -1, 1 };
        return Random.Range(minRotateSpeed, maxRotateSpeed) * direction[Random.Range(0, 1)];
    }
}
