using System.Collections;
using UnityEngine;

namespace AndesSim
{
    // Controla el servidor SWIFT: color, contorno, elevación, partículas y estado de selección.
    public class ServerHighlightManager : MonoBehaviour
    {
        public bool IsSelected { get; private set; }
        public Transform ServerRoot { get; private set; }

        private Renderer serverRenderer;
        private Renderer outlineRenderer;
        private ParticleEffectManager particles;
        private GameObject selectionRing;
        private Vector3 restPosition;
        private float outlineIntensity = 0.55f;

        public void Configure(Transform root, Renderer body, Renderer outline,
            ParticleEffectManager particleManager)
        {
            ServerRoot = root;
            serverRenderer = body;
            outlineRenderer = outline;
            particles = particleManager;
            restPosition = root.position;

            selectionRing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            selectionRing.name = "Server_SelectionRing";
            selectionRing.transform.SetParent(root, false);
            selectionRing.transform.localPosition = new Vector3(0f, -0.12f, 0f);
            selectionRing.transform.localScale = new Vector3(0.24f, 0.003f, 0.24f);
            Destroy(selectionRing.GetComponent<Collider>());
            selectionRing.GetComponent<Renderer>().material =
                MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.Cyan, 0.9f), true);
            selectionRing.SetActive(false);
        }

        public void SetColor(Color color)
        {
            if (serverRenderer != null) MaterialFactory.SetColor(serverRenderer.material, color);
        }

        public void Pulse(Color color, float duration = 1.2f)
        {
            AndesCoroutines.Run(PulseRoutine(color, duration));
        }

        private IEnumerator PulseRoutine(Color color, float duration)
        {
            if (serverRenderer == null) yield break;
            var material = serverRenderer.material;
            var original = MaterialFactory.GetColor(material);
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / Mathf.Max(0.01f, duration))
            {
                var k = (Mathf.Sin(t * Mathf.PI * 4f - Mathf.PI * 0.5f) + 1f) * 0.5f;
                MaterialFactory.SetColor(material, Color.Lerp(original, color, k));
                yield return null;
            }
            MaterialFactory.SetColor(material, original);
        }

        public IEnumerator BeginIdentification()
        {
            if (ServerRoot == null) yield break;
            SetColor(AndesPalette.Red);
            if (outlineRenderer != null)
                MaterialFactory.SetColor(outlineRenderer.material, AndesPalette.Alpha(AndesPalette.Red, outlineIntensity));
            particles?.StartOrbit(AndesPalette.Red);

            var from = ServerRoot.position;
            var to = restPosition + Vector3.up * 0.16f;
            if (HologramAnimationManager.Instance != null)
                yield return HologramAnimationManager.Instance.Move(ServerRoot, from, to, 2.2f);
            else ServerRoot.position = to;

            Pulse(AndesPalette.Red, 1.6f);
        }

        public void SetHover(bool hover)
        {
            if (IsSelected || outlineRenderer == null) return;
            outlineIntensity = hover ? 1f : 0.55f;
            MaterialFactory.SetColor(outlineRenderer.material, AndesPalette.Alpha(AndesPalette.Red, outlineIntensity));
        }

        public void Select()
        {
            if (IsSelected) return;
            IsSelected = true;
            AndesCoroutines.Run(SelectRoutine());
        }

        private IEnumerator SelectRoutine()
        {
            SetColor(AndesPalette.TextBlueWhite);
            if (outlineRenderer != null)
                MaterialFactory.SetColor(outlineRenderer.material, AndesPalette.Alpha(AndesPalette.TextBlueWhite, 1f));
            particles?.Concentrate();
            if (selectionRing != null) selectionRing.SetActive(true);

            var original = ServerRoot.localScale;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 0.5f)
            {
                var k = Mathf.Sin(t * Mathf.PI);
                ServerRoot.localScale = original * (1f + 0.12f * k);
                yield return null;
            }
            ServerRoot.localScale = original;

            if (HologramAnimationManager.Instance != null)
                HologramAnimationManager.Instance.Play(
                    HologramAnimationManager.Instance.Wave(ServerRoot.position, AndesPalette.TextBlueWhite, 0.35f, 1.1f));
            AudioManager.Instance?.PlayConfirm();
        }
    }
}
