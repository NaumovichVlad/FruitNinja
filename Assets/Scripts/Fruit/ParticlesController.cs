using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    private readonly List<TransparentedParticles> _particles = new List<TransparentedParticles>();

    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private SpriteRenderer juiceDropSprite;
    [SerializeField] private string sortingLayerName;
    [SerializeField] private int minParticlesCount;
    [SerializeField] private int maxParticlesCount;
    [SerializeField] private float minParticleTransparentSpeed;
    [SerializeField] private float maxParticleTransparentSpeed;
    [SerializeField] private float minParticleMoveSpeed;
    [SerializeField] private float maxParticleMoveSpeed;
    [SerializeField] private float minParticleScale;
    [SerializeField] private float maxParticleScale;
    [SerializeField] private float minParticleScatterRadius;
    [SerializeField] private float maxParticleScatterRadius;
    [SerializeField] private float maxRotationAngle;
    [SerializeField] private float maxJuiceDropSize;
    [SerializeField] private float minJuiceDropSize;
    [SerializeField] private float juiceDropSpeed;
    [SerializeField] private int juiceDropCount;


    private class TransparentedParticles
    {
        public GameObject Particle;

        public float ParticleMoveSpeed;

        public float TransparentSpeed;

        public SpriteRenderer ParticleRenderer;
    }
    public void CreateParticles(GameObject cuttedObject, Sprite particleSprite, Color juiceColor)
    {
        var particleCount = Random.Range(minParticlesCount, maxParticlesCount + 1);

        for (var i = 0; i < particleCount; i++)
        {
            particlePrefab.transform.position = CalculateRandomPosition(cuttedObject.transform.position);
            particlePrefab.transform.localScale = CalculateRandomScale(minParticleScale, maxParticleScale);

            var particle = new TransparentedParticles()
            {
                Particle = Instantiate(particlePrefab),
                ParticleMoveSpeed = Random.Range(minParticleMoveSpeed, maxParticleMoveSpeed),
                TransparentSpeed = Random.Range(minParticleTransparentSpeed, maxParticleTransparentSpeed)
            };

            particle.ParticleRenderer = particle.Particle.AddComponent<SpriteRenderer>();
            particle.ParticleRenderer.sortingLayerName = sortingLayerName;
            particle.ParticleRenderer.sprite = particleSprite;
            particle.Particle.transform.Rotate(new Vector3(0, 0, maxRotationAngle * Random.value));

            _particles.Add(particle);
        }

        CreateDrops(cuttedObject, juiceColor);
    }

    private void CreateDrops(GameObject cuttedObject, Color juiceColor)
    {
        for (var i = 0; i < juiceDropCount; i++)
        {

            juiceDropSprite.color = juiceColor;
            var drop = Instantiate(juiceDropSprite.gameObject);
            drop.transform.position = cuttedObject.transform.position;
            drop.transform.localScale = CalculateRandomScale(minJuiceDropSize, maxJuiceDropSize);
            var angle = Random.value * 360;

            MoveController.GetInstance().AddMovingObject(new MoveController.MovingObject()
            {
                Instance = drop,
                Direction = new Vector2(juiceDropSpeed * Mathf.Cos(Mathf.Deg2Rad * angle), juiceDropSpeed * Mathf.Sin(Mathf.Deg2Rad * angle)),
                RotationSpeed = 0
            }) ;
        }
    }

    private Vector2 CalculateRandomPosition(Vector2 cutPossition)
    {
        var randomPosition = new Vector2();
        var randomScatter = Random.Range(minParticleScatterRadius, maxParticleScatterRadius);

        randomPosition.x = Random.Range(cutPossition.x - randomScatter, cutPossition.x + randomScatter);
        randomPosition.y = Random.Range(cutPossition.y - randomScatter, cutPossition.y + randomScatter);

        return randomPosition;
    }

    private Vector3 CalculateRandomScale(float min, float max)
    {
        var randomScale = Random.Range(min, max);

        return new Vector3(randomScale, randomScale, randomScale);
    }

    private void Update()
    {
        for (var i = 0; i < _particles.Count; i++)
        {

            _particles[i].Particle.transform.Translate(new Vector2(0, -_particles[i].ParticleMoveSpeed * Time.deltaTime), Space.World);

            var color = _particles[i].ParticleRenderer.material.color;

            color.a -= _particles[i].TransparentSpeed * Time.deltaTime;
            _particles[i].ParticleRenderer.material.SetColor("_Color", color);

            if (color.a < 0)
            {
                Destroy(_particles[i].Particle);
                _particles.RemoveAt(i--);
            }
        }
    }
}
