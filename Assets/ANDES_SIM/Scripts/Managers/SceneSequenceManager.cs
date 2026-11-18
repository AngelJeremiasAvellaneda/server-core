using System;
using System.Collections;
using UnityEngine;

namespace AndesSim
{
    // Orquesta la secuencia de 12 pasos esperando al usuario en cada interacción importante.
    public class SceneSequenceManager : MonoBehaviour
    {
        private InfrastructureBuilder infrastructure;
        private HolographicPanel currentPanel;
        private bool running;

        public void StartSequence(InfrastructureBuilder builder)
        {
            if (running) return;
            running = true;
            infrastructure = builder;
            AndesCoroutines.Run(Sequence());
        }

        private IEnumerator Sequence()
        {
            // 1. Preparación del entorno y 2. materialización de la infraestructura.
            yield return infrastructure.Materialize();

            // 3. Presentación del sistema.
            yield return WelcomeStep();

            // 4. Primera alerta de seguridad.
            yield return AlertStep();

            // 5. Acceso sospechoso a las 20:36.
            yield return AccessStep();

            // 6. Operaciones irregulares.
            yield return OperationsStep();

            // 7. Anomalía de la impresora.
            yield return PrinterStep();

            // 8. Registros incompletos.
            yield return LogsStep();

            // 9. Reconstrucción visual.
            yield return ReconstructionStep();

            // 10. Misión principal.
            yield return MissionStep();

            // 11. Identificación del servidor SWIFT.
            yield return IdentificationStep();

            // 12. Selección del servidor y cierre de la escena.
            yield return SelectionStep();
            yield return FinalStep();
        }

        private IEnumerator CloseCurrent()
        {
            if (currentPanel == null) yield break;
            yield return currentPanel.CloseRoutine();
            currentPanel = null;
        }

        private static PanelConfig NewConfig(string title, string message, Color accent)
        {
            return new PanelConfig { Title = title, Message = message, Accent = accent };
        }

        private IEnumerator WelcomeStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.WelcomeTitle,
                AndesContent.WelcomeLine1 + "\n\n" + AndesContent.WelcomeLine2, AndesPalette.Cyan);
            config.ShowContinue = false;
            config.Buttons.Add(new ButtonConfig(AndesContent.WelcomeButton, () =>
            {
                if (HologramAnimationManager.Instance != null)
                    HologramAnimationManager.Instance.Play(HologramAnimationManager.Instance.Wave(
                        Vector3.up * 0.76f, AndesPalette.Cyan, 0.85f, 1.2f));
                infrastructure.PulseRacks();
                infrastructure.IlluminateConnections();
                proceed = true;
            }));

            currentPanel = HolographicPanel.Create<NotificationPanel>(
                config, new Vector3(0f, 1.30f, 0.30f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            while (!proceed) yield return null;
        }

        private IEnumerator AlertStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.AlertTitle,
                AndesContent.AlertLine1 + "\n\n" + AndesContent.AlertLine2, AndesPalette.Amber);
            config.Status = AndesContent.AlertStatus;
            config.ShowContinue = false;
            config.AutoDismiss = true;
            config.DisplaySeconds = 6f;
            config.OnDismissed = () => proceed = true;
            config.Buttons.Add(new ButtonConfig(AndesContent.Continue, () => proceed = true, true));

            if (HologramAnimationManager.Instance != null)
                HologramAnimationManager.Instance.Play(HologramAnimationManager.Instance.Wave(
                    Vector3.up * 0.76f, AndesPalette.Amber, 0.75f, 1.4f));

            currentPanel = HolographicPanel.Create<SecurityAlertPanel>(
                config, new Vector3(0f, 1.30f, 0.28f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            while (!proceed) yield return null;
        }

        private IEnumerator AccessStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.AccessTitle, AndesContent.AccessLine, AndesPalette.Amber);
            config.Status = AndesContent.AccessClassification;
            config.SecondaryMessage = AndesContent.AccessDescription;
            config.ShowContinue = false;
            config.Buttons.Add(new ButtonConfig(AndesContent.ViewDetail, () =>
            {
                currentPanel.ShowDetail(AndesContent.AccessDetailTitle, AndesContent.AccessDetailLines,
                    () => proceed = true);
            }));

            currentPanel = HolographicPanel.Create<AccessLogPanel>(
                config, new Vector3(-0.42f, 1.28f, 0.24f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            ((AccessLogPanel)currentPanel).PlayClock();
            infrastructure.PulseTerminal();
            infrastructure.TraceAccessLine();
            while (!proceed) yield return null;
        }

        private IEnumerator OperationsStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.OpsTitle, AndesContent.OpsLine, AndesPalette.Red);
            config.SecondaryMessage = AndesContent.OpsNote;
            config.ShowContinue = false;
            config.Buttons.Add(new ButtonConfig(AndesContent.ViewDetail, () =>
                currentPanel.ShowDetail("ANÁLISIS DE OPERACIONES", AndesContent.OpsDetailLines,
                    () => proceed = true)));

            currentPanel = HolographicPanel.Create<TransactionPanel>(
                config, new Vector3(0.42f, 1.28f, 0.24f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            ((TransactionPanel)currentPanel).PlayCounter();
            infrastructure.PulseTerminal();
            while (!proceed) yield return null;
        }

        private IEnumerator PrinterStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.PrinterTitle,
                AndesContent.PrinterLine1 + "\n\n" + AndesContent.PrinterLine2, AndesPalette.Amber);
            config.Status = AndesContent.PrinterStatus;
            config.ShowContinue = false;
            config.Buttons.Add(new ButtonConfig(AndesContent.ViewDetail, () =>
                currentPanel.ShowDetail(AndesContent.PrinterDetailTitle, AndesContent.PrinterDetailLines,
                    () => proceed = true)));

            currentPanel = HolographicPanel.Create<PrinterAlertPanel>(
                config, new Vector3(0.52f, 1.22f, 0.22f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            infrastructure.PrinterFeed();
            if (HologramAnimationManager.Instance != null)
                HologramAnimationManager.Instance.Play(HologramAnimationManager.Instance.Wave(
                    infrastructure.Printer.position, AndesPalette.Amber, 0.28f, 1.3f));
            while (!proceed) yield return null;
        }

        private IEnumerator LogsStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.LogsTitle,
                AndesContent.LogsLine1 + "\n" + AndesContent.LogsLine2, AndesPalette.Amber);
            config.Status = AndesContent.LogsStatus;
            config.Buttons.Add(new ButtonConfig(AndesContent.Continue, () => proceed = true, true));

            currentPanel = HolographicPanel.Create<LogAnalysisPanel>(
                config, new Vector3(-0.48f, 1.22f, 0.22f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            while (!proceed) yield return null;
        }

        private IEnumerator ReconstructionStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.ReconstructionTitle, AndesContent.ReconstructionLine, AndesPalette.ElectricBlue);
            config.SecondaryMessage = AndesContent.ReconstructionNote;
            config.Buttons.Add(new ButtonConfig(AndesContent.Continue, () => proceed = true, true));

            currentPanel = HolographicPanel.Create<IncidentReconstructionPanel>(
                config, new Vector3(0f, 1.30f, 0.26f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            while (!proceed) yield return null;
        }

        private IEnumerator MissionStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.MissionTitle, "", AndesPalette.Cyan);
            config.Status = "MISIÓN PRINCIPAL";
            config.ShowContinue = false;
            config.Buttons.Add(new ButtonConfig(AndesContent.MissionButton, () =>
            {
                infrastructure.ScanRacks();
                proceed = true;
            }));

            currentPanel = HolographicPanel.Create<MissionPanel>(
                config, new Vector3(0f, 1.38f, 0.30f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            while (!proceed) yield return null;
            yield return infrastructure.ScanRacks();
            infrastructure.Server.Pulse(AndesPalette.Cyan, 1.5f);
        }

        private IEnumerator IdentificationStep()
        {
            yield return CloseCurrent();
            var proceed = false;
            var config = NewConfig(AndesContent.ServerTitle,
                AndesContent.ServerLine1 + "\n\n" + AndesContent.ServerLine2, AndesPalette.Red);
            config.Status = AndesContent.ServerPriority;
            config.Buttons.Add(new ButtonConfig(AndesContent.Continue, () => proceed = true, true));

            currentPanel = HolographicPanel.Create<ServerIdentificationPanel>(
                config, new Vector3(0f, 1.40f, 0.30f), true, () => proceed = true);
            yield return currentPanel.PlayShow();
            yield return infrastructure.Server.BeginIdentification();
            while (!proceed) yield return null;
        }

        private IEnumerator SelectionStep()
        {
            yield return CloseCurrent();
            var config = NewConfig(AndesContent.HandTitle,
                AndesContent.HandLine1 + "\n\n" + AndesContent.HandLine2, AndesPalette.ElectricBlue);
            config.ShowContinue = false;
            currentPanel = HolographicPanel.Create<HandInteractionIndicator>(
                config, new Vector3(0f, 1.42f, 0.30f), false, null);
            yield return currentPanel.PlayShow();

            var target = infrastructure.Server.ServerRoot.GetComponent<ServerSelectionTarget>();
            while (target == null || !target.Selected) yield return null;
        }

        private IEnumerator FinalStep()
        {
            yield return CloseCurrent();
            var config = NewConfig(AndesContent.SelectedTitle,
                AndesContent.SelectedLine1 + "\n\n" + AndesContent.SelectedLine2, AndesPalette.TextBlueWhite);
            config.ShowContinue = false;
            currentPanel = HolographicPanel.Create<NotificationPanel>(
                config, new Vector3(0f, 1.42f, 0.30f), false, null);
            yield return currentPanel.PlayShow();
        }
    }
}
