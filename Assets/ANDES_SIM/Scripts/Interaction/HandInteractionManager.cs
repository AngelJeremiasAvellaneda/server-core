using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace AndesSim
{
    // Gestiona PINCH / POKE / HOVER con manos XR y fallback por ratón y teclado para el editor.
    public class HandInteractionManager : MonoBehaviour
    {
        public static HandInteractionManager Instance { get; private set; }

        private readonly List<IAndesInteractable> targets = new List<IAndesInteractable>();
        private Camera cam;
        private IAndesInteractable hovered;
        private bool previousPinch;

        public string ActiveMode { get; private set; } = "Ratón / teclado";

        private void Awake()
        {
            Instance = this;
            cam = Camera.main;
        }

        public void Register(IAndesInteractable target)
        {
            if (target != null && !targets.Contains(target)) targets.Add(target);
        }

        public void Unregister(IAndesInteractable target)
        {
            if (target != null) targets.Remove(target);
        }

        private void Update()
        {
            if (cam == null) cam = Camera.main;
            Vector3? handPoint = GetHandPoint(out var pinch);
            Vector2 pointer;
            bool select;
            bool tabPressed = false;

            if (handPoint.HasValue)
            {
                ActiveMode = "Hand Tracking XR";
                pointer = cam != null ? cam.WorldToScreenPoint(handPoint.Value) : Vector2.zero;
                select = pinch && !previousPinch;
                previousPinch = pinch;
            }
            else
            {
                ActiveMode = "Ratón / teclado";
                pointer = Vector2.zero;
                select = false;
#if ENABLE_INPUT_SYSTEM
                if (UnityEngine.InputSystem.Mouse.current != null)
                {
                    pointer = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                    select = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
                }
                if (UnityEngine.InputSystem.Keyboard.current != null)
                {
                    select |= UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
                    tabPressed = UnityEngine.InputSystem.Keyboard.current.tabKey.wasPressedThisFrame;
                }
#else
                pointer = Input.mousePosition;
                select = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
                tabPressed = Input.GetKeyDown(KeyCode.Tab);
#endif
                previousPinch = false;
            }

            var best = FindBest(pointer, handPoint);
            if (best != hovered)
            {
                hovered?.SetHover(false);
                hovered = best;
                hovered?.SetHover(true);
            }

            if (select && hovered != null)
                hovered.Select();

            if (tabPressed)
                CycleFocus();
        }

        private IAndesInteractable FindBest(Vector2 pointer, Vector3? handPoint)
        {
            IAndesInteractable best = null;
            var bestScore = float.MaxValue;
            foreach (var target in targets)
            {
                if (target == null || !target.IsEnabled) continue;
                var world = target.InteractionPoint;
                float score;
                if (handPoint.HasValue)
                {
                    score = Vector3.Distance(world, handPoint.Value);
                    if (score > Mathf.Max(0.08f, target.InteractionRadius * 2.5f)) continue;
                }
                else
                {
                    if (cam == null) continue;
                    var screen = cam.WorldToScreenPoint(world);
                    if (screen.z <= 0f) continue;
                    score = Vector2.Distance(pointer, new Vector2(screen.x, screen.y));
                    if (score > 110f) continue;
                }
                if (score < bestScore)
                {
                    bestScore = score;
                    best = target;
                }
            }
            return best;
        }

        private void CycleFocus()
        {
            if (targets.Count == 0) return;
            var index = hovered != null ? targets.IndexOf(hovered) : -1;
            hovered?.SetHover(false);
            hovered = targets[(index + 1) % targets.Count];
            hovered.SetHover(true);
        }

        // Usa el subsistema genérico de XR de Unity. Si no hay dispositivos de mano, devuelve null.
        private static Vector3? GetHandPoint(out bool pinch)
        {
            pinch = false;
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(
                InputDeviceCharacteristics.HandTracking | InputDeviceCharacteristics.Right, devices);
            if (devices.Count == 0)
                InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HandTracking, devices);
            if (devices.Count == 0) return null;

            var device = devices[0];
            if (!device.TryGetFeatureValue(CommonUsages.devicePosition, out var position)) return null;
            if (device.TryGetFeatureValue(CommonUsages.trigger, out var trigger)) pinch = trigger > 0.65f;
            if (!pinch && device.TryGetFeatureValue(CommonUsages.grip, out var grip)) pinch = grip > 0.75f;
            return position;
        }
    }
}
