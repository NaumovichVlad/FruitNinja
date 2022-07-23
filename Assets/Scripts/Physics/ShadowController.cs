using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    private readonly List<GameObject> _shadows = new List<GameObject>();
    private static ShadowController shadowControllerinstance;

    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private float distanceFromObject;
    [SerializeField] private float shadowTransparency;

    private void Awake()
    {
        shadowControllerinstance = this;
    }

    public static ShadowController GetInstance()
    {
        return shadowControllerinstance;
    }

    public void CreateShadow(GameObject instance)
    {
        var shadowSprite = shadowPrefab.GetComponent<SpriteRenderer>();
        var color = Color.black;

        color.a = shadowTransparency;
        shadowSprite.color = color;
        shadowSprite.sprite = instance.GetComponent<SpriteRenderer>().sprite;

        var shadow = Instantiate(shadowPrefab);
        shadow.transform.SetParent(instance.transform, false);

        _shadows.Add(shadow);
    }

    public void AddShadow(GameObject shadow)
    {
        _shadows.Add(shadow);
    }

    void Update()
    {
        MoveAndRotateShadows();
    }

    private void MoveAndRotateShadows()
    {
        for (var i = 0; i < _shadows.Count; i++)
        {
            if (_shadows[i] == null)
            {
                _shadows.RemoveAt(i--);
            }
            else
            {
                var parentPosition = new Vector2(_shadows[i].transform.parent.transform.position.x, _shadows[i].transform.parent.transform.position.y - distanceFromObject);

                _shadows[i].transform.position = parentPosition;
            }
        }
    }
}
