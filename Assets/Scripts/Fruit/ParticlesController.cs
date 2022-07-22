using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    private readonly List<TransparentedParticles> _particles = new List<TransparentedParticles>();

    [SerializeField] private GameObject particlePrefab;
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

    private class TransparentedParticles
    {
        public GameObject Particle;

        public float ParticleMoveSpeed;

        public float TransparentSpeed;

        public SpriteRenderer ParticleRenderer;
    }
    public void CreateParticles(GameObject cuttedObject, Sprite particleSprite)
    {
        var particleCount = Random.Range(minParticlesCount, maxParticlesCount + 1);

        for (var i = 0; i < particleCount; i++)
        {
            particlePrefab.transform.position = CalculateRandomPosition(cuttedObject.transform.position);
            particlePrefab.transform.localScale = CalculateRandomScale();

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

    }

    private Vector2 CalculateRandomPosition(Vector2 cutPossition)
    {
        var randomPosition = new Vector2();
        var randomScatter = Random.Range(minParticleScatterRadius, maxParticleScatterRadius);

        randomPosition.x = Random.Range(cutPossition.x - randomScatter, cutPossition.x + randomScatter);
        randomPosition.y = Random.Range(cutPossition.y - randomScatter, cutPossition.y + randomScatter);

        return randomPosition;
    }

    private Vector3 CalculateRandomScale()
    {
        var randomScale = Random.Range(minParticleScale, maxParticleScale);

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
