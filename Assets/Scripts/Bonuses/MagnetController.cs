using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    [SerializeField] private Sprite magnetSprite;
    [SerializeField] private float magnetPower;
    [SerializeField] private float magnetRadius;
    [SerializeField] private int magnetTime;

    private bool _isEnabled;
    private float _timer;

    private MoveController.MovingObject _states;
    private float _swipeSpeed;

    private void Start()
    {
        InitializeMagnet();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void InitializeMagnet()
    {
        ShadowController.GetInstance().CreateShadow(gameObject, magnetSprite);
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        if (CalculateLength((Vector2)gameObject.transform.position, swipePosition) < gameObject.transform.localScale.x)
        {
            SwipeDetection.SwipeEvent -= OnSwipe;

            var states = MoveController.GetInstance().PeekMovingObject(gameObject);

            MoveController.GetInstance().AddMagnetization(gameObject.transform.position, magnetTime, magnetPower, magnetRadius);

            states.RotationSpeed = 0;
            states.Direction = Vector2.zero;

            _states = states;
            _swipeSpeed = swipeSpeed;
            _isEnabled = true;
        }
    }

    private void Update()
    {
        if (_isEnabled)
        {
            if (_timer < magnetTime)
            {
                _timer += Time.fixedDeltaTime;
            }
            else
            {
                _isEnabled = false;

                CuttingController.GetInstance().Cut(_states, magnetSprite, _swipeSpeed);

                Destroy(gameObject);
            }
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
