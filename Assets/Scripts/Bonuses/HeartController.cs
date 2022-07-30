using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private float healthSpeed;

    private void Start()
    {
        InitializeHeart();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void InitializeHeart()
    {
        ShadowController.GetInstance().CreateShadow(gameObject, heartSprite);
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        if (CalculateLength((Vector2)gameObject.transform.position, swipePosition) < gameObject.transform.localScale.x)
        {
            var states = MoveController.GetInstance().PeekMovingObject(gameObject);

            CuttingController.GetInstance().Cut(states, heartSprite, swipeSpeed);

            HealthController.GetInstance().AddHealth(transform.position, healthSpeed);

            Destroy(gameObject);
        }
    }

    private float CalculateLength(Vector2 firstVector, Vector2 secondVector)
    {
        return Mathf.Sqrt(Mathf.Pow(firstVector.x - secondVector.x, 2) + Mathf.Pow(firstVector.y - secondVector.y, 2));
    }

    private void OnDestroy()
    {
        SwipeDetection.SwipeEvent -= OnSwipe;
    }
}
