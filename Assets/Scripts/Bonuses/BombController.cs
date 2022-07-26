using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private List<Sprite> bombPartsSprites;
    [SerializeField] private FruitHalfController fruitHalfController;
    [SerializeField] private float explosionPower;

    private readonly List<GameObject> _bombParts = new List<GameObject>();

    public static event OnExplosion ExplosionEvent;
    public delegate void OnExplosion(Vector2 explosionPosition, float explosionPower);

    void Start()
    {
        InitializeBomb();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void InitializeBomb()
    {
        foreach (var part in bombPartsSprites)
        {
            fruitHalfController.CreateNewHalf(part);

            var partInstance = Instantiate(fruitHalfController.gameObject);

            partInstance.transform.SetParent(gameObject.transform, false);
            ShadowController.GetInstance().CreateShadow(partInstance);

            _bombParts.Add(partInstance);
        }
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        if (CalculateLength((Vector2)gameObject.transform.position, swipePosition) < gameObject.transform.localScale.x)
        {
            var states = MoveController.GetInstance().PeekMovingObject(gameObject);
            
            LaunchParts(states);

            explosionPrefab.transform.position = gameObject.transform.position;

            ExplosionEvent(gameObject.transform.position, explosionPower);

            Instantiate(explosionPrefab);

            Destroy(gameObject);
        }
    }

    private void LaunchParts(MoveController.MovingObject states)
    {
        const float angle = 360;
        var launchAngle = angle / _bombParts.Count;

        for (var i = 0; i < _bombParts.Count; i++)
        {
            var partInstance = new MoveController.MovingObject()
            {
                Instance = Instantiate(_bombParts[i].gameObject),
                RotationSpeed = states.RotationSpeed,
                Direction = new Vector2(
                    states.Direction.x * Mathf.Cos(Mathf.Deg2Rad * launchAngle * i), states.Direction.y * Mathf.Sin(Mathf.Deg2Rad * launchAngle * i))
            };

            partInstance.Instance.transform.position = gameObject.transform.position;
            partInstance.Instance.transform.rotation = gameObject.transform.rotation;

            MoveController.GetInstance().AddMovingObject(partInstance);
            ShadowController.GetInstance().AddShadow(partInstance.Instance.transform.GetChild(0).gameObject);
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
