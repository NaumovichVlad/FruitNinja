using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    [System.Serializable]
    private class FruitSprites
    {
        public Sprite FirstHalf;

        public Sprite SecondHalf;

        public Sprite Particle;
    }

    private FruitSprites _fruit;
    private readonly List<GameObject> halfs = new List<GameObject>();

    [SerializeField] private List<FruitSprites> fruitSprites;
    [SerializeField] private FruitHalfController fruitHalfCreator;

    public static event OnCut CutEvent;
    public delegate void OnCut(MoveController.MovingObject cutObject, List<GameObject> halfs, Sprite particle, Vector2 cutDirection, float cutSpeed);

    private void InitializeFruit()
    {
        _fruit = GetRandomFruitSprite();
        AddHalf(_fruit.FirstHalf);
        AddHalf(_fruit.SecondHalf);

    }

    private void AddHalf(Sprite half)
    {
        fruitHalfCreator.CreateNewHalf(half);
        var halfInstance = Instantiate(fruitHalfCreator.gameObject);
        halfInstance.transform.SetParent(gameObject.transform, false);
        halfs.Add(halfInstance);
        ShadowController.GetInstance().CreateShadow(halfInstance);
    }

    private FruitSprites GetRandomFruitSprite()
    {
        return fruitSprites[Random.Range(0, fruitSprites.Count)];
    }

    private void Start()
    {
        InitializeFruit();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        if (CalculateLength((Vector2)gameObject.transform.position, swipePosition) < gameObject.transform.localScale.x)
        {
            var states = MoveController.GetInstance().PeekMovingObject(gameObject);

            CutEvent(states, halfs, _fruit.Particle, direction, swipeSpeed);
            ScoreCounterController.GetInstance().AddScore(gameObject);
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
