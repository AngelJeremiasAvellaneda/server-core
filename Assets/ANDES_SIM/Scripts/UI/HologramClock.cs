using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AndesSim
{
    // Reloj holográfico que termina indicando las 20:36.
    public class HologramClock : MonoBehaviour
    {
        private Text text;

        public static HologramClock Build(Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 offsetMin, Vector2 offsetMax)
        {
            var go = new GameObject("HologramClock", typeof(RectTransform), typeof(HologramClock));
            go.transform.SetParent(parent, false);
            var rect = (RectTransform)go.transform;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            var clock = go.GetComponent<HologramClock>();
            clock.text = UiFactory.CreateText(go.transform, "Time", "20:30", 46, AndesPalette.Cyan,
                TextAnchor.MiddleCenter, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, true);
            return clock;
        }

        public void Play()
        {
            AndesCoroutines.Run(Animate());
        }

        private IEnumerator Animate()
        {
            for (var minute = 30; minute <= 36; minute++)
            {
                text.text = "20:" + minute.ToString("00");
                yield return new WaitForSecondsRealtime(0.12f);
            }
            text.text = AndesContent.AccessTime;
        }
    }
}
