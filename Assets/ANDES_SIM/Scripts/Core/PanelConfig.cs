using System;
using System.Collections.Generic;
using UnityEngine;

namespace AndesSim
{
    [Serializable]
    public class ButtonConfig
    {
        public string Label;
        public Action Action;
        public bool ClosePanel = false;

        public ButtonConfig(string label, Action action, bool closePanel = false)
        {
            Label = label;
            Action = action;
            ClosePanel = closePanel;
        }
    }

    // Configuración reutilizable para cualquier panel holográfico del proyecto.
    [Serializable]
    public class PanelConfig
    {
        public string Title = string.Empty;
        public string Status = string.Empty;
        public string Icon = "◈";
        public string Message = string.Empty;
        public string SecondaryMessage = string.Empty;
        public Color Accent = AndesPalette.Cyan;
        public float WidthMeters = 1.30f;
        public float HeightMeters = 0.72f;
        public bool AutoDismiss = false;
        public float DisplaySeconds = 6f;
        public bool ShowContinue = true;
        public List<ButtonConfig> Buttons = new List<ButtonConfig>();
        public Action OnDismissed;

        public PanelConfig Set(string title, string message, Color accent)
        {
            Title = title;
            Message = message;
            Accent = accent;
            return this;
        }
    }
}
