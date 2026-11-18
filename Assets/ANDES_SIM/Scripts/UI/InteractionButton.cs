using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AndesSim
{
    // Botón holográfico compatible con PINCH, POKE, HOVER y con el modo de prueba por ratón.
    public class InteractionButton : MonoBehaviour, IAndesInteractable
    {
        public string Label { get; private set; }
        public Action Action { get; private set; }
        public bool IsEnabled { get; private set; } = true;

        private Image fill;
        private Image border;
        private Image pulse;
        private Text labelText;
        private RectTransform ring;
        private Color accent;
        private bool hovered;

        public Vector3 InteractionPoint => transform.position;
        public float InteractionRadius => 0.075f;

        public static InteractionButton Build(Transform parent, string label, Action action,
            Vector2 anchoredPosition, Vector2 size, Color accentColor)
        {
            var go = new GameObject("Button_" + label, typeof(RectTransform), typeof(InteractionButton));
            go.transform.SetParent(parent, false);
            var rect = (RectTransform)go.transform;
            rect.anchorMin = new Vector2(0f, 0.5f);
            rect.anchorMax = new Vector2(0f, 0.5f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.sizeDelta = size;
            rect.anchoredPosition = anchoredPosition;

            var button = go.GetComponent<InteractionButton>();
            button.Label = label;
            button.Action = action;
            button.accent = accentColor;
            button.BuildVisual(label);
            HandInteractionManager.Instance?.Register(button);
            return button;
        }

        private void BuildVisual(string label)
        {
            fill = UiFactory.CreateImage(transform, "Fill", AndesPalette.Alpha(AndesPalette.DeepBlue, 0.95f),
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            border = UiFactory.CreateImage(transform, "Border", AndesPalette.Alpha(accent, 0.85f),
                Vector2.zero, Vector2.one, new Vector2(3f, 3f), new Vector2(-3f, -3f));
            fill.transform.SetAsLastSibling();
            pulse = UiFactory.CreateImage(transform, "Pulse", AndesPalette.Alpha(accent, 0f),
                Vector2.zero, Vector2.one, new Vector2(-4f, -4f), new Vector2(4f, 4f));
            pulse.transform.SetAsFirstSibling();

            labelText = UiFactory.CreateText(transform, "Label", label, 24, AndesPalette.TextBlueWhite,
                TextAnchor.MiddleCenter, Vector2.zero, Vector2.one, new Vector2(8f, 4f), new Vector2(-8f, -4f), true);

            ring = UiFactory.CreateContainer(transform, "HoverRing", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(-16f, -16f), new Vector2(16f, 16f));
            var ringImage = UiFactory.CreateImage(ring, "Ring", AndesPalette.Alpha(AndesPalette.Cyan, 0f),
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            ringImage.name = "RingImage";
        }

        public void SetHover(bool value)
        {
            if (!IsEnabled || hovered == value) return;
            hovered = value;
            border.color = value ? accent : AndesPalette.Alpha(accent, 0.85f);
            labelText.color = value ? Color.white : AndesPalette.TextBlueWhite;
            transform.localScale = value ? Vector3.one * 1.04f : Vector3.one;
            var ringImage = ring.GetChild(0).GetComponent<Image>();
            ringImage.color = value ? AndesPalette.Alpha(AndesPalette.Cyan, 0.9f) : AndesPalette.Alpha(AndesPalette.Cyan, 0f);
        }

        public void Select()
        {
            if (!IsEnabled) return;
            IsEnabled = false;
            AndesCoroutines.Run(SelectRoutine());
        }

        private IEnumerator SelectRoutine()
        {
            var start = fill.color;
            fill.color = AndesPalette.Alpha(accent, 0.75f);
            var rt = (RectTransform)transform;
            var original = rt.sizeDelta;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 0.18f)
            {
                var k = Mathf.Sin(t * Mathf.PI);
                rt.sizeDelta = original * (1f + 0.08f * k);
                yield return null;
            }
            rt.sizeDelta = original;
            fill.color = start;
            Action?.Invoke();
            IsEnabled = true;
        }

        private void OnDestroy()
        {
            HandInteractionManager.Instance?.Unregister(this);
        }
    }
}
