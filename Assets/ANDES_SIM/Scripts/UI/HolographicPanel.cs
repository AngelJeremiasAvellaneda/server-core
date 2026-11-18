using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AndesSim
{
    // Componente base reutilizable: fondo, borde, icono, textos, botones, animaciones y ficha ampliada.
    public class HolographicPanel : MonoBehaviour
    {
        public PanelConfig Config { get; private set; }
        public RectTransform Root { get; private set; }
        public CanvasGroup Group { get; private set; }
        public RectTransform ExtraContainer { get; private set; }
        public RectTransform ButtonContainer { get; private set; }
        public Text TitleText { get; private set; }
        public Text StatusText { get; private set; }
        public Text MessageText { get; private set; }
        public Text SecondaryText { get; private set; }
        public Text IconText { get; private set; }
        public Image BorderImage { get; private set; }
        public Image ScanLine { get; private set; }

        private GameObject detailOverlay;
        private Action onContinue;
        private bool waitForUser;
        private bool built;

        public static T Create<T>(PanelConfig config, Vector3 position, bool waitForUser = true,
            Action onContinue = null) where T : HolographicPanel
        {
            var go = new GameObject(typeof(T).Name, typeof(RectTransform), typeof(Canvas), typeof(CanvasGroup), typeof(T));
            go.transform.position = position;
            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.sortingOrder = 30;
            var panel = go.GetComponent<T>();
            panel.Initialize(config, waitForUser, onContinue);
            return panel;
        }

        public virtual void Initialize(PanelConfig config, bool waitForUser, Action onContinue)
        {
            if (built) return;
            Config = config ?? new PanelConfig();
            this.waitForUser = waitForUser;
            this.onContinue = onContinue;
            built = true;

            var canvas = GetComponent<Canvas>();
            Root = (RectTransform)transform;
            var ratio = Mathf.Max(0.35f, Config.HeightMeters / Config.WidthMeters);
            Root.sizeDelta = new Vector2(1000f, 1000f * ratio);
            Root.localScale = Vector3.one * (Config.WidthMeters / 1000f);

            Group = GetComponent<CanvasGroup>();
            Group.alpha = 0f;
            Group.interactable = true;
            Group.blocksRaycasts = true;

            BuildFrame();
            BuildTexts();
            BuildExtraContent();
            BuildButtons();
            gameObject.AddComponent<Billboard>();
        }

        private void BuildFrame()
        {
            UiFactory.CreateImage(Root, "OuterGlow", AndesPalette.Alpha(Config.Accent, 0.07f),
                Vector2.zero, Vector2.one, new Vector2(-18f, -18f), new Vector2(18f, 18f));

            UiFactory.CreateImage(Root, "PanelFill", AndesPalette.Alpha(AndesPalette.DeepBlue, 0.94f),
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            BorderImage = UiFactory.CreateImage(Root, "Border", AndesPalette.Alpha(Config.Accent, 0.92f),
                Vector2.zero, Vector2.one, new Vector2(4f, 4f), new Vector2(-4f, -4f));
            UiFactory.CreateImage(Root, "InnerFill", AndesPalette.Alpha(AndesPalette.DeepBlueSolid, 0.98f),
                Vector2.zero, Vector2.one, new Vector2(9f, 9f), new Vector2(-9f, -9f));

            UiFactory.CreateImage(Root, "HeaderBar", AndesPalette.Alpha(Config.Accent, 0.16f),
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(10f, -86f), new Vector2(-10f, -10f));

            UiFactory.CreateImage(Root, "AccentStrip", Config.Accent,
                new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(10f, 14f), new Vector2(15f, -14f));

            ScanLine = UiFactory.CreateImage(Root, "ScanLine", AndesPalette.Alpha(Config.Accent, 0.35f),
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(12f, -94f), new Vector2(-12f, -90f));
        }

        private void BuildTexts()
        {
            IconText = UiFactory.CreateText(Root, "Icon", Config.Icon, 40, Config.Accent,
                TextAnchor.MiddleCenter, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(26f, -78f), new Vector2(92f, -14f), true);

            TitleText = UiFactory.CreateText(Root, "Title", Config.Title, 32, AndesPalette.TextBlueWhite,
                TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(104f, -76f), new Vector2(-250f, -14f), true);

            StatusText = UiFactory.CreateText(Root, "Status", Config.Status, 22, Config.Accent,
                TextAnchor.MiddleRight, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(-260f, -74f), new Vector2(-26f, -16f), true);

            MessageText = UiFactory.CreateText(Root, "Message", Config.Message, 27, AndesPalette.TextBlueWhite,
                TextAnchor.UpperLeft, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(42f, -190f), new Vector2(-42f, -98f));

            SecondaryText = UiFactory.CreateText(Root, "Secondary", Config.SecondaryMessage, 22,
                AndesPalette.Alpha(AndesPalette.TextBlueWhite, 0.85f), TextAnchor.UpperLeft,
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(42f, -246f), new Vector2(-42f, -190f));

            ExtraContainer = UiFactory.CreateContainer(Root, "Extra", new Vector2(0f, 0f), new Vector2(1f, 1f),
                new Vector2(38f, 110f), new Vector2(-38f, -250f));
            ButtonContainer = UiFactory.CreateContainer(Root, "Buttons", new Vector2(0f, 0f), new Vector2(1f, 0f),
                new Vector2(30f, 18f), new Vector2(-30f, 96f));
        }

        protected virtual void BuildExtraContent()
        {
        }

        private void BuildButtons()
        {
            var width = 260f;
            var gap = 20f;
            var x = 0f;
            foreach (var buttonConfig in Config.Buttons)
            {
                var captured = buttonConfig;
                InteractionButton.Build(ButtonContainer, captured.Label, () =>
                {
                    captured.Action?.Invoke();
                    if (captured.ClosePanel) Close();
                }, new Vector2(x, 0f), new Vector2(width, 62f), Config.Accent);
                x += width + gap;
            }

            if (waitForUser && Config.ShowContinue)
            {
                InteractionButton.Build(ButtonContainer, AndesContent.Continue, () => onContinue?.Invoke(),
                    new Vector2(x, 0f), new Vector2(210f, 62f), AndesPalette.ElectricBlue);
            }
        }

        public Coroutine PlayShow()
        {
            return AndesCoroutines.Run(ShowRoutine());
        }

        private IEnumerator ShowRoutine()
        {
            gameObject.SetActive(true);
            Group.alpha = 0f;
            Root.localScale = Vector3.one * (Config.WidthMeters / 1000f) * 0.86f;
            var targetScale = Vector3.one * (Config.WidthMeters / 1000f);
            var duration = 0.55f;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / duration)
            {
                var eased = Mathf.SmoothStep(0f, 1f, t);
                Group.alpha = eased;
                Root.localScale = Vector3.Lerp(Root.localScale, targetScale, 0.18f);
                yield return null;
            }
            Group.alpha = 1f;
            Root.localScale = targetScale;
            AndesCoroutines.Run(ScanRoutine());
            AndesCoroutines.Run(BorderPulseRoutine());

            if (Config.AutoDismiss)
            {
                yield return new WaitForSecondsRealtime(Config.DisplaySeconds);
                yield return CloseRoutine();
                Config.OnDismissed?.Invoke();
            }
        }

        private IEnumerator ScanRoutine()
        {
            var rect = (RectTransform)ScanLine.transform;
            var start = rect.anchoredPosition;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 2.4f)
            {
                var y = Mathf.Lerp(Root.rect.height * 0.42f, -Root.rect.height * 0.42f, t);
                rect.anchoredPosition = new Vector2(start.x, y);
                yield return null;
            }
        }

        private IEnumerator BorderPulseRoutine()
        {
            var baseColor = AndesPalette.Alpha(Config.Accent, 0.92f);
            while (gameObject.activeInHierarchy)
            {
                for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 1.8f)
                {
                    var k = (Mathf.Sin(t * Mathf.PI * 2f) + 1f) * 0.5f;
                    BorderImage.color = Color.Lerp(baseColor, AndesPalette.Alpha(Color.white, 0.95f), k * 0.35f);
                    yield return null;
                }
            }
        }

        public Coroutine Close()
        {
            return AndesCoroutines.Run(CloseRoutine());
        }

        public IEnumerator CloseRoutine()
        {
            if (!gameObject.activeInHierarchy) yield break;
            var start = Group.alpha;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 0.5f)
            {
                Group.alpha = Mathf.Lerp(start, 0f, Mathf.SmoothStep(0f, 1f, t));
                Root.localScale *= 1f - 0.4f * Time.unscaledDeltaTime;
                yield return null;
            }
            Group.alpha = 0f;
            gameObject.SetActive(false);
        }

        // Ficha ampliada reutilizable para registros, operaciones e impresora.
        public void ShowDetail(string detailTitle, string[] lines, Action onClose)
        {
            if (detailOverlay != null) Destroy(detailOverlay);
            detailOverlay = new GameObject("DetailOverlay", typeof(RectTransform), typeof(Image));
            detailOverlay.transform.SetParent(Root, false);
            var rect = (RectTransform)detailOverlay.transform;
            rect.anchorMin = new Vector2(0.06f, 0.10f);
            rect.anchorMax = new Vector2(0.94f, 0.90f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var image = detailOverlay.GetComponent<Image>();
            image.color = AndesPalette.Alpha(AndesPalette.DeepBlueSolid, 0.99f);
            image.raycastTarget = false;

            UiFactory.CreateImage(detailOverlay.transform, "DetailBorder", AndesPalette.Alpha(Config.Accent, 0.95f),
                Vector2.zero, Vector2.one, new Vector2(3f, 3f), new Vector2(-3f, -3f)).transform.SetAsFirstSibling();

            UiFactory.CreateText(detailOverlay.transform, "DetailTitle", detailTitle, 30, Config.Accent,
                TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(28f, -72f), new Vector2(-28f, -16f), true);

            var content = string.Join("\n", lines ?? Array.Empty<string>());
            UiFactory.CreateText(detailOverlay.transform, "DetailLines", content, 23, AndesPalette.TextBlueWhite,
                TextAnchor.UpperLeft, Vector2.zero, Vector2.one, new Vector2(28f, 92f), new Vector2(-28f, -84f));

            InteractionButton.Build((RectTransform)detailOverlay.transform, AndesContent.Close, () =>
            {
                Destroy(detailOverlay);
                detailOverlay = null;
                onClose?.Invoke();
            }, new Vector2(0f, 0f), new Vector2(200f, 58f), Config.Accent);
        }
    }
}
