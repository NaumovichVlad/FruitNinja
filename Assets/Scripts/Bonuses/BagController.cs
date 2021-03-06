using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BagController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer partRenderer;
    [SerializeField] private List<Sprite> partSprites;
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private int fruitCount;
    [SerializeField] private float fruitSpeed;
    [SerializeField] private float fruitRotationSpeed;

    private readonly List<GameObject> _parts = new List<GameObject>();

    private void Start()
    {
        InitializeBag();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void InitializeBag()
    {
        foreach (var part in partSprites)
        {
            partRenderer.sprite = part;

            var partInstance = Instantiate(partRenderer.gameObject);

            partInstance.transform.SetParent(gameObject.transform, false);
            ShadowController.GetInstance().CreateShadow(partInstance, part);

            partInstance.transform.position = gameObject.transform.position;
            partInstance.transform.rotation = gameObject.transform.rotation;

            _parts.Add(partInstance);
        }
    }

    private void InitializeFruits()
    {
        var angle = 180 / fruitCount;

        for (var i = 0; i < fruitCount; i++)
        {
            var direction = new Vector3(
                fruitSpeed * Mathf.Cos(Mathf.Deg2Rad * angle * (i + 1)), fruitSpeed * Mathf.Sin(Mathf.Deg2Rad * angle * (i + 1)));

            fruitPrefab.transform.position = gameObject.transform.position;
            fruitPrefab.transform.Translate(fruitPrefab.transform.localScale);

            MoveController.GetInstance().AddMovingObject(new MoveController.MovingObject()
            {
                IsHealth = true,
                Instance = Instantiate(fruitPrefab),
                RotationSpeed = fruitRotationSpeed,
                Direction = direction
            });
        }
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        if (CalculateLength((Vector2)gameObject.transform.position, swipePosition) < gameObject.transform.localScale.x)
        {
            var states = MoveController.GetInstance().PeekMovingObject(gameObject);

            partSprites.RemoveAt(1);

            CuttingController.GetInstance().Cut(states, partSprites, swipeSpeed);

            InitializeFruits();

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
