using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AndesSim
{
    // Panel genérico de notificación del sistema.
    public class NotificationPanel : HolographicPanel
    {
    }

    // Alerta de seguridad con icono de advertencia y expansión suave.
    public class SecurityAlertPanel : HolographicPanel
    {
        protected override void BuildExtraContent()
        {
            UiFactory.CreateText(ExtraContainer, "Warning", "⚠", 54, AndesPalette.Amber,
                TextAnchor.MiddleLeft, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(10f, -40f), new Vector2(90f, 40f), true);
        }
    }

    // Registro de acceso con reloj holográfico y línea luminosa hacia la infraestructura.
    public class AccessLogPanel : HolographicPanel
    {
        private HologramClock clock;

        protected override void BuildExtraContent()
        {
            clock = HologramClock.Build(ExtraContainer, new Vector2(0f, 0.35f), new Vector2(0.34f, 1f),
                new Vector2(0f, 0f), new Vector2(0f, 0f));
            UiFactory.CreateText(ExtraContainer, "ClockLabel", "HORA DEL EVENTO", 18, AndesPalette.Cyan,
                TextAnchor.LowerCenter, new Vector2(0f, 0f), new Vector2(0.34f, 0.38f), Vector2.zero, Vector2.zero);
            UiFactory.CreateText(ExtraContainer, "AccessDescription", AndesContent.AccessDescription, 23,
                AndesPalette.TextBlueWhite, TextAnchor.UpperLeft, new Vector2(0.38f, 0.45f), new Vector2(1f, 1f),
                Vector2.zero, Vector2.zero);
            UiFactory.CreateText(ExtraContainer, "Classification", AndesContent.AccessClassification, 24,
                AndesPalette.Amber, TextAnchor.MiddleLeft, new Vector2(0.38f, 0f), new Vector2(1f, 0.35f),
                Vector2.zero, Vector2.zero, true);
        }

        public void PlayClock()
        {
            clock?.Play();
        }
    }

    // Monitoreo de operaciones: contador hasta 36 y valor monetario posterior.
    public class TransactionPanel : HolographicPanel
    {
        private Text counter;
        private Text value;

        protected override void BuildExtraContent()
        {
            counter = UiFactory.CreateText(ExtraContainer, "Counter", "0 ÓRDENES", 58, AndesPalette.Red,
                TextAnchor.MiddleCenter, new Vector2(0f, 0.42f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero, true);
            value = UiFactory.CreateText(ExtraContainer, "Value", "", 36, AndesPalette.ElectricBlue,
                TextAnchor.MiddleCenter, new Vector2(0f, 0f), new Vector2(1f, 0.42f), Vector2.zero, Vector2.zero, true);
        }

        public void PlayCounter()
        {
            AndesCoroutines.Run(CounterRoutine());
        }

        private IEnumerator CounterRoutine()
        {
            for (var i = 1; i <= 36; i++)
            {
                if (counter != null) counter.text = i + " ÓRDENES";
                yield return new WaitForSecondsRealtime(0.055f);
            }
            yield return new WaitForSecondsRealtime(0.35f);
            if (value != null) value.text = AndesContent.OpsValue;
        }
    }

    // Anomalía de la impresora de confirmaciones.
    public class PrinterAlertPanel : HolographicPanel
    {
        protected override void BuildExtraContent()
        {
            UiFactory.CreateText(ExtraContainer, "Warning", "⚠", 54, AndesPalette.Amber,
                TextAnchor.MiddleLeft, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(10f, -40f), new Vector2(90f, 40f), true);
            UiFactory.CreateText(ExtraContainer, "Note", AndesContent.PrinterNote, 20,
                AndesPalette.Alpha(AndesPalette.TextBlueWhite, 0.85f), TextAnchor.UpperLeft,
                new Vector2(0.1f, 0f), new Vector2(1f, 0.6f), Vector2.zero, Vector2.zero);
        }
    }

    // Registros incompletos: fragmentos, ausencias y efecto de escaneo visual.
    public class LogAnalysisPanel : HolographicPanel
    {
        protected override void BuildExtraContent()
        {
            var lines = new[]
            {
                "[REGISTRO]  Fragmento disponible  ·  ▒▒▒▒▒▒",
                "[REGISTRO]  Segmento no disponible  ·  DATOS AUSENTES",
                "[REGISTRO]  Fragmento disponible  ·  ▒▒▒▒▒▒",
                "[REGISTRO]  Tramos no recuperables  ·  TRAZABILIDAD LIMITADA"
            };
            for (var i = 0; i < lines.Length; i++)
            {
                var color = i % 2 == 0 ? AndesPalette.Cyan : AndesPalette.Amber;
                UiFactory.CreateText(ExtraContainer, "LogLine_" + i, lines[i], 20, color,
                    TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 1f),
                    new Vector2(10f, 120f - i * 42f), new Vector2(-10f, 154f - i * 42f));
            }
        }
    }

    // Reconstrucción visual: cuatro anomalías relacionadas mediante conexiones holográficas.
    public class IncidentReconstructionPanel : HolographicPanel
    {
        protected override void BuildExtraContent()
        {
            var labels = new[] { "ACCESO 20:36", "36 ÓRDENES", "IMPRESORA", "REGISTROS" };
            for (var i = 0; i < labels.Length; i++)
            {
                var x = 12f + i * 230f;
                UiFactory.CreateImage(ExtraContainer, "Node_" + i, AndesPalette.Alpha(AndesPalette.ElectricBlue, 0.35f),
                    new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(x, -46f), new Vector2(x + 184f, 46f));
                UiFactory.CreateText(ExtraContainer, "NodeLabel_" + i, labels[i], 19, AndesPalette.TextBlueWhite,
                    TextAnchor.MiddleCenter, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                    new Vector2(x + 6f, -40f), new Vector2(x + 178f, 40f), true);
                if (i < labels.Length - 1)
                {
                    UiFactory.CreateImage(ExtraContainer, "Link_" + i, AndesPalette.Alpha(AndesPalette.Cyan, 0.9f),
                        new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                        new Vector2(x + 184f, -2f), new Vector2(x + 226f, 2f));
                }
            }
        }
    }

    // Panel central de misión.
    public class MissionPanel : HolographicPanel
    {
        protected override void BuildExtraContent()
        {
            UiFactory.CreateText(ExtraContainer, "Objective", AndesContent.MissionMain, 42, AndesPalette.Cyan,
                TextAnchor.MiddleCenter, new Vector2(0f, 0.35f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero, true);
            UiFactory.CreateText(ExtraContainer, "ObjectiveDescription", AndesContent.MissionDescription, 22,
                AndesPalette.TextBlueWhite, TextAnchor.UpperCenter, new Vector2(0f, 0f), new Vector2(1f, 0.35f),
                Vector2.zero, Vector2.zero);
        }
    }

    // Identificación del servidor SWIFT con prioridad ALTA.
    public class ServerIdentificationPanel : HolographicPanel
    {
        protected override void BuildExtraContent()
        {
            UiFactory.CreateText(ExtraContainer, "ServerName", AndesContent.ServerName, 34, AndesPalette.Red,
                TextAnchor.MiddleCenter, new Vector2(0f, 0.45f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero, true);
            UiFactory.CreateText(ExtraContainer, "Priority", AndesContent.ServerPriorityLabel + " " + AndesContent.ServerPriority,
                26, AndesPalette.Red, TextAnchor.MiddleCenter, new Vector2(0f, 0f), new Vector2(1f, 0.45f),
                Vector2.zero, Vector2.zero, true);
        }
    }

    // Indicador de seguimiento de manos con gesto de pinza animado.
    public class HandInteractionIndicator : HolographicPanel
    {
        private RectTransform fingerA;
        private RectTransform fingerB;

        protected override void BuildExtraContent()
        {
            var a = UiFactory.CreateImage(ExtraContainer, "FingerA", AndesPalette.Alpha(AndesPalette.Cyan, 0.95f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-46f, -12f), new Vector2(-18f, 12f));
            var b = UiFactory.CreateImage(ExtraContainer, "FingerB", AndesPalette.Alpha(AndesPalette.Cyan, 0.95f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(18f, -12f), new Vector2(46f, 12f));
            fingerA = (RectTransform)a.transform;
            fingerB = (RectTransform)b.transform;
            UiFactory.CreateText(ExtraContainer, "GestureLabel", "PINZCA  ·  SELECCIONA", 20, AndesPalette.ElectricBlue,
                TextAnchor.LowerCenter, new Vector2(0f, 0f), new Vector2(1f, 0.35f), Vector2.zero, Vector2.zero, true);
        }

        private void Update()
        {
            if (fingerA == null || fingerB == null) return;
            var k = (Mathf.Sin(Time.time * 2.2f) + 1f) * 0.5f;
            var gap = Mathf.Lerp(34f, 4f, k);
            fingerA.anchoredPosition = new Vector2(-gap, 0f);
            fingerB.anchoredPosition = new Vector2(gap, 0f);
        }
    }
}
