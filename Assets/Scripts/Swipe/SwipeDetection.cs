using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction, Vector2 swipePosition, float swipeSpeed);

    [SerializeField] private float minSwipeSpeed;
    [SerializeField] private float minSwipeLength;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SwipeRenderer swipeRenderer;

    private Vector2 tapPosition;
    private Vector2 swipePosition;
    private Vector2 velocity;

    private bool isSwiping;
    private bool isMobile;

    void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    void Update()
    {
        if (isMobile)
        {
            if(Input.touchCount > 0)
            {
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    tapPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    swipeRenderer.AddSwipeLine(tapPosition);
                }
                else if(Input.GetTouch(0).phase == TouchPhase.Canceled
                    || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ResetSwipe();
                }
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                tapPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                swipeRenderer.AddSwipeLine(tapPosition);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                ResetSwipe();
            }
        }

        CheckSwipe();
    }

    private void CheckSwipe()
    {
        var speed = velocity.magnitude;

        if (isSwiping)
        {
            if (isMobile && Input.touchCount > 0)
            {
                swipePosition = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else if (Input.GetMouseButton(0))
            {
                swipePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            velocity = swipePosition - tapPosition;
            Debug.Log(velocity.magnitude);
        }

        speed = Mathf.Abs(speed - velocity.magnitude);

        if (velocity.magnitude > minSwipeLength && speed > minSwipeSpeed)
        {
            swipeRenderer.ContinueSwipeLine(swipePosition);

            if (SwipeEvent != null)
            {
                SwipeEvent(velocity, swipePosition, speed);
            }

        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;
        swipeRenderer.EndSwipeLine();
        tapPosition = Vector2.zero;
        velocity = Vector2.zero;
    }
}

