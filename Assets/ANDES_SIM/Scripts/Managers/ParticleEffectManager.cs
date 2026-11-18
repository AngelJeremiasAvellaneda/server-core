using System.Collections;
using UnityEngine;

namespace AndesSim
{
    // Partículas orbitales del servidor SWIFT y concentración final tras la selección.
    public class ParticleEffectManager : MonoBehaviour
    {
        private Transform host;
        private readonly System.Collections.Generic.List<Transform> particles = new System.Collections.Generic.List<Transform>();
        private Color currentColor = AndesPalette.Red;
        private bool concentrated;

        public void Configure(Transform hostTransform)
        {
            host = hostTransform;
            for (var i = 0; i < 14; i++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.name = "OrbitParticle_" + i;
                Destroy(go.GetComponent<Collider>());
                go.transform.SetParent(host, false);
                go.transform.localScale = Vector3.one * 0.012f;
                go.GetComponent<Renderer>().material = MaterialFactory.Create(AndesPalette.Alpha(currentColor, 0.85f), true);
                particles.Add(go.transform);
            }
            StopAll();
        }

        public void StartOrbit(Color color)
        {
            currentColor = color;
            concentrated = false;
            foreach (var particle in particles)
            {
                particle.gameObject.SetActive(true);
                MaterialFactory.SetColor(particle.GetComponent<Renderer>().material, AndesPalette.Alpha(color, 0.85f));
            }
        }

        public void Concentrate()
        {
            concentrated = true;
        }

        public void StopAll()
        {
            foreach (var particle in particles) particle.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (host == null || particles.Count == 0) return;
            var time = Time.time;
            for (var i = 0; i < particles.Count; i++)
            {
                var particle = particles[i];
                if (!particle.gameObject.activeSelf) continue;
                var angle = time * 0.9f + i * Mathf.PI * 2f / particles.Count;
                var radius = concentrated ? 0.015f : 0.14f + 0.02f * Mathf.Sin(time * 1.7f + i);
                var height = concentrated ? 0f : Mathf.Sin(time * 1.2f + i) * 0.09f;
                particle.localPosition = new Vector3(Mathf.Cos(angle) * radius, height, Mathf.Sin(angle) * radius);
            }
        }
    }
}
