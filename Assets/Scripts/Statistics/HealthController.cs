using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    private static HealthController instance;
    private readonly Stack<GameObject> _healthes = new Stack<GameObject> ();
    private readonly List<GameObject> _removedHealth = new List<GameObject> ();

    public static event LosePopUpController.OnLose LoseEvent;

    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private int startHealthCount;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float startScale;
    [SerializeField] private float animationScale;
    [SerializeField] private float animationSpeed;
    [SerializeField] private LosePopUpController popUpController;

    private void Awake()
    {
        LosePopUpController.RestartEvent += InitializeHealth;
        instance = this;
        InitializeHealth();
    }

    public static HealthController GetInstance()
    {
        return instance;
    }

    private void InitializeHealth()
    {
        for (var i = 0; i < startHealthCount; i++)
        {
            healthPrefab.transform.localScale = new Vector3(startScale, startScale, startScale);

            var health = Instantiate(healthPrefab, gameObject.transform);

            health.transform.Translate(new Vector2 (-i * health.transform.lossyScale.x, 0));

            _healthes.Push(health);
        }
    }

    public void RemoveHealth()
    {
        if (_healthes.Count > 0)
        {
            if (_healthes.Count == 1)
            {
                LoseEvent();
                Instantiate(popUpController.gameObject);
                ScoreCounterController.GetInstance().SaveProgress();
            }

            _removedHealth.Add(_healthes.Pop());
        }
    }

    private void Update()
    {
        for (var j = 0; j < _removedHealth.Count; j++)
        {
            if (_removedHealth[j].transform.localScale.x > 0)
            {
                _removedHealth[j].transform.localScale -= 
                    new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(_removedHealth[j]);
                _removedHealth.RemoveAt(j--);
            }
        }

        foreach (var health in _healthes)
        {
            Animate(health);
        }
    }

    private void Animate(GameObject health)
    {
        var anim = animationSpeed * Time.deltaTime;
        if (health.transform.localScale.x > startScale - animationScale)
        {
            health.transform.localScale -= new Vector3(anim, anim, anim);
        }
        else
        {
            health.transform.localScale = new Vector3(startScale, startScale, startScale);
        }
    }
}
