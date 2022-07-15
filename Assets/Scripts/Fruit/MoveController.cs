using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Vector2 Direction;

    [SerializeField] private Vector2 attractiveForce;

    void FixedUpdate()
    {
        transform.Translate(Direction);
         Direction += attractiveForce;
    }
}
