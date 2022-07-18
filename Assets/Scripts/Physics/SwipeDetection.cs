using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction, Vector2 swipePosition);

    [SerializeField] private float minSwipeLength;
    [SerializeField] private Camera mainCamera;

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
        velocity = Vector2.zero;

        if (isSwiping)
        {
            if (isMobile && Input.touchCount > 0)
            {
                velocity = (Vector2)mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position) - tapPosition;
                swipePosition = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);

            }
            else if (Input.GetMouseButton(0))
            {
                velocity = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - tapPosition;
                swipePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (velocity.magnitude > minSwipeLength)
        {
            if (SwipeEvent != null)
            {
                SwipeEvent(velocity, swipePosition);
            }

        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;

        tapPosition = Vector2.zero;
        velocity = Vector2.zero;
    }
}

