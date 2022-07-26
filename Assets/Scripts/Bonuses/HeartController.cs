using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private FruitHalfController fruitHalfController;
    [SerializeField] private SpriteRenderer heartRenderer;
    [SerializeField] private List<Sprite> partSprites;
    [SerializeField] private float healthSpeed;

    private readonly List<GameObject> _parts = new List<GameObject>();

    private void Start()
    {
        InitializeHeart();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void InitializeHeart()
    {
        foreach (var part in partSprites)
        {
            fruitHalfController.CreateNewHalf(part);

            var partInstance = Instantiate(fruitHalfController.gameObject);

            partInstance.transform.SetParent(gameObject.transform, false);
            ShadowController.GetInstance().CreateShadow(partInstance);

            _parts.Add(partInstance);
        }
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        if (CalculateLength((Vector2)gameObject.transform.position, swipePosition) < gameObject.transform.localScale.x)
        {
            var states = MoveController.GetInstance().PeekMovingObject(gameObject);

            CuttingController.GetInstance().Cut(states, _parts, swipeSpeed);

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
