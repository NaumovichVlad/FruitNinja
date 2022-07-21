using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitHalfController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    public void CreateNewHalf(Sprite sprite)
    {
        renderer.sprite = sprite;
    }
}
