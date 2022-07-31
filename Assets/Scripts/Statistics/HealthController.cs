using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    private static HealthController instance;
    private int _healthesInRow;
    private readonly Stack<GameObject> _healthes = new Stack<GameObject> ();
    private readonly List<GameObject> _removedHealth = new List<GameObject> ();
    private readonly List<MovedHealth> _movedHealth = new List<MovedHealth>();

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
        _healthesInRow = (int)(Screen.width / 2 / startScale);
        LosePopUpController.RestartEvent += InitializeHealth;
        BombController.ExplosionEvent += OnExplode;

        instance = this;
        InitializeHealth();
    }

    private void OnExplode(Vector2 explosionPosition, float explosionPower)
    {
        RemoveHealth();
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
            healthPrefab.transform.position = GetNextPosition();

            var health = Instantiate(healthPrefab, gameObject.transform);

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

                StartCoroutine(WaitLose());
            }

            _removedHealth.Add(_healthes.Pop());
        }
    }

    IEnumerator WaitLose()
    {
        yield return new WaitWhile(() =>
            MoveController.GetInstance().GetMovingObjectCount() > 0);

        Instantiate(popUpController.gameObject);
        ScoreCounterController.GetInstance().SaveProgress();
    }

    public void AddHealth(Vector2 position, float speed)
    {
        healthPrefab.transform.position = position;

        var health = Instantiate(healthPrefab, gameObject.transform, true);
        health.transform.localScale = new Vector3(startScale, startScale, startScale);

        var endPosition = gameObject.transform.position;

        _movedHealth.Add(new MovedHealth()
        {
            Health = health,
            XSpeed = (endPosition.x - position.x) * speed,
            YSpeed = (endPosition.y - position.y) * speed,
            
        });
    }

    private Vector2 GetNextPosition()
    {
        var nextPosition = new Vector2();

        nextPosition.x = gameObject.transform.position.x - startScale * (_healthes.Count % _healthesInRow);
        nextPosition.y = gameObject.transform.position.y - startScale * (_healthes.Count / _healthesInRow);

        return nextPosition;
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

        for (var i = 0; i < _movedHealth.Count; i++)
        {
            if (_movedHealth[i].Health.transform.position.x > gameObject.transform.position.x
                || _movedHealth[i].Health.transform.position.y > gameObject.transform.position.y)
            {
                _movedHealth[i].Health.transform.localPosition = GetNextPosition();
                _healthes.Push(_movedHealth[i].Health);
                _movedHealth.RemoveAt(i--);
            }
            else
            {
                _movedHealth[i].Health.transform.Translate(new Vector2(_movedHealth[i].XSpeed, _movedHealth[i].YSpeed) * Time.deltaTime);
            }
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

    void OnDestroy()
    {
        LosePopUpController.RestartEvent -= InitializeHealth;
        BombController.ExplosionEvent -= OnExplode;
    }

    private class MovedHealth
    {
        public GameObject Health;

        public float XSpeed;

        public float YSpeed;

        public Vector2 EndPosition;
    }
}
