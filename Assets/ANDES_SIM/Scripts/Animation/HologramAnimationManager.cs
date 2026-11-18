using System.Collections;
using UnityEngine;

namespace AndesSim
{
    // Animaciones suaves reutilizables: fade, escala, pulso, onda, escaneo y desplazamiento.
    public class HologramAnimationManager : MonoBehaviour
    {
        public static HologramAnimationManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public Coroutine Play(IEnumerator routine)
        {
            return AndesCoroutines.Run(routine);
        }

        public IEnumerator Fade(CanvasGroup group, float from, float to, float duration)
        {
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / Mathf.Max(0.01f, duration))
            {
                group.alpha = Mathf.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
            group.alpha = to;
        }

        public IEnumerator Move(Transform target, Vector3 from, Vector3 to, float duration)
        {
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / Mathf.Max(0.01f, duration))
            {
                target.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
            target.position = to;
        }

        public IEnumerator Scale(Transform target, Vector3 from, Vector3 to, float duration)
        {
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / Mathf.Max(0.01f, duration))
            {
                target.localScale = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
            target.localScale = to;
        }

        public IEnumerator PulseColor(Renderer renderer, Color from, Color to, float duration, int cycles = 1)
        {
            if (renderer == null) yield break;
            var material = renderer.material;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / Mathf.Max(0.01f, duration))
            {
                var k = (Mathf.Sin(t * Mathf.PI * 2f * cycles - Mathf.PI * 0.5f) + 1f) * 0.5f;
                MaterialFactory.SetColor(material, Color.Lerp(from, to, k));
                yield return null;
            }
            MaterialFactory.SetColor(material, from);
        }

        public IEnumerator Wave(Vector3 center, Color color, float maxRadius, float duration)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = "Andes_Wave";
            go.transform.position = center + Vector3.up * 0.01f;
            Destroy(go.GetComponent<Collider>());
            var material = MaterialFactory.Create(AndesPalette.Alpha(color, 0.45f), true);
            go.GetComponent<Renderer>().material = material;
            var start = new Vector3(0.05f, 0.004f, 0.05f);
            go.transform.localScale = start;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / Mathf.Max(0.01f, duration))
            {
                var r = Mathf.Lerp(0.05f, maxRadius, Mathf.SmoothStep(0f, 1f, t));
                go.transform.localScale = new Vector3(r, 0.004f, r);
                MaterialFactory.SetColor(material, AndesPalette.Alpha(color, 0.45f * (1f - t)));
                yield return null;
            }
            Destroy(go);
        }

        public IEnumerator ScanArea(Vector3 center, Vector3 size, Color color, float duration)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "Andes_ScanPlane";
            go.GetComponent<Renderer>().material = MaterialFactory.Create(AndesPalette.Alpha(color, 0.22f), true);
            Destroy(go.GetComponent<Collider>());
            go.transform.localScale = new Vector3(size.x, 0.004f, size.z * 0.06f);
            var start = center - Vector3.forward * size.z * 0.5f;
            var end = center + Vector3.forward * size.z * 0.5f;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / Mathf.Max(0.01f, duration))
            {
                go.transform.position = Vector3.Lerp(start, end, t) + Vector3.up * 0.06f;
                yield return null;
            }
            Destroy(go);
        }
    }
}
