using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitHalfController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void CreateNewHalf(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
