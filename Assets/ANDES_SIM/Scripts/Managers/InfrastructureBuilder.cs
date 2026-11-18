using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;

namespace AndesSim
{
    // Construye la maqueta holográfica con primitivas de Unity y expone sus efectos.
    public class InfrastructureBuilder : MonoBehaviour
    {
        public Transform InfrastructureRoot { get; private set; }
        public Transform SwiftTerminal { get; private set; }
        public Transform Printer { get; private set; }
        public ServerHighlightManager Server { get; private set; }

        private readonly List<Renderer> racks = new List<Renderer>();
        private readonly List<LineRenderer> connections = new List<LineRenderer>();
        private readonly List<Material> connectionMaterials = new List<Material>();
        private Renderer terminalRenderer;
        private Renderer printerIndicator;
        private Transform paper;
        private Vector3 paperRest;

        private static readonly Vector3 ViewerPosition = new Vector3(0f, 1.6f, -2.5f);
        private static readonly Vector3 PanelFocus = new Vector3(0f, 1.32f, 0.3f);

        public void Build()
        {
            InfrastructureRoot = new GameObject("Infrastructure").transform;
            InfrastructureRoot.SetParent(transform, false);

            BuildTable();
            BuildRacks();
            BuildTerminal();
            BuildPrinter();
            BuildServer();
            BuildConnections();
            EnsureCameraAndLight();
        }

        private void BuildTable()
        {
            var table = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            table.name = "Table_Hologram";
            table.transform.SetParent(InfrastructureRoot, false);
            table.transform.position = new Vector3(0f, 0.74f, 0f);
            table.transform.localScale = new Vector3(0.82f, 0.012f, 0.82f);
            Destroy(table.GetComponent<Collider>());
            table.GetComponent<Renderer>().material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.DeepBlue, 0.55f), true);

            var rim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rim.name = "Table_Rim";
            rim.transform.SetParent(InfrastructureRoot, false);
            rim.transform.position = new Vector3(0f, 0.754f, 0f);
            rim.transform.localScale = new Vector3(0.84f, 0.002f, 0.84f);
            Destroy(rim.GetComponent<Collider>());
            rim.GetComponent<Renderer>().material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.Cyan, 0.7f), true);
        }

        private void BuildRacks()
        {
            for (var i = 0; i < 4; i++)
            {
                var rack = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rack.name = "Rack_" + (i + 1);
                rack.transform.SetParent(InfrastructureRoot, false);
                rack.transform.position = new Vector3(-0.39f + i * 0.26f, 0.88f, 0f);
                rack.transform.localScale = new Vector3(0.18f, 0.24f, 0.16f);
                Destroy(rack.GetComponent<Collider>());
                var renderer = rack.GetComponent<Renderer>();
                renderer.material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.Cyan, 0.55f), true);
                racks.Add(renderer);
            }
        }

        private void BuildTerminal()
        {
            var terminal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            terminal.name = "Terminal_SWIFT";
            terminal.transform.SetParent(InfrastructureRoot, false);
            terminal.transform.position = new Vector3(-0.56f, 0.84f, 0.30f);
            terminal.transform.localScale = new Vector3(0.20f, 0.13f, 0.05f);
            Destroy(terminal.GetComponent<Collider>());
            terminalRenderer = terminal.GetComponent<Renderer>();
            terminalRenderer.material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.ElectricBlue, 0.7f), true);
            SwiftTerminal = terminal.transform;
        }

        private void BuildPrinter()
        {
            var printer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            printer.name = "Printer_Confirmations";
            printer.transform.SetParent(InfrastructureRoot, false);
            printer.transform.position = new Vector3(0.56f, 0.82f, 0.30f);
            printer.transform.localScale = new Vector3(0.18f, 0.10f, 0.14f);
            Destroy(printer.GetComponent<Collider>());
            printer.GetComponent<Renderer>().material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.ElectricBlue, 0.6f), true);
            Printer = printer.transform;

            var indicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            indicator.name = "Printer_Indicator";
            indicator.transform.SetParent(printer.transform, false);
            indicator.transform.localPosition = new Vector3(0.06f, 0.055f, 0.04f);
            indicator.transform.localScale = Vector3.one * 0.018f;
            Destroy(indicator.GetComponent<Collider>());
            printerIndicator = indicator.GetComponent<Renderer>();
            printerIndicator.material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.Cyan, 0.9f), true);

            paper = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            paper.name = "Printer_Paper";
            paper.SetParent(printer.transform, false);
            paper.localPosition = new Vector3(0f, 0.06f, 0f);
            paper.localScale = new Vector3(0.12f, 0.001f, 0.08f);
            Destroy(paper.GetComponent<Collider>());
            paper.GetComponent<Renderer>().material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.TextBlueWhite, 0.85f), true);
            paper.gameObject.SetActive(false);
            paperRest = paper.localPosition;
        }

        private void BuildServer()
        {
            var server = GameObject.CreatePrimitive(PrimitiveType.Cube);
            server.name = "Server_SWIFT";
            server.transform.SetParent(InfrastructureRoot, false);
            server.transform.position = new Vector3(0.22f, 1.00f, 0.02f);
            server.transform.localScale = new Vector3(0.16f, 0.26f, 0.14f);
            Destroy(server.GetComponent<Collider>());
            var body = server.GetComponent<Renderer>();
            body.material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.Cyan, 0.8f), true);

            var outline = GameObject.CreatePrimitive(PrimitiveType.Cube);
            outline.name = "Server_Outline";
            outline.transform.SetParent(server.transform, false);
            outline.transform.localPosition = Vector3.zero;
            outline.transform.localScale = Vector3.one * 1.10f;
            Destroy(outline.GetComponent<Collider>());
            var outlineRenderer = outline.GetComponent<Renderer>();
            outlineRenderer.material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.Cyan, 0.18f), true);

            var particles = gameObject.AddComponent<ParticleEffectManager>();
            particles.Configure(server.transform);

            Server = gameObject.AddComponent<ServerHighlightManager>();
            Server.Configure(server.transform, body, outlineRenderer, particles);

            var target = server.AddComponent<ServerSelectionTarget>();
            target.Configure(Server);
        }

        private void BuildConnections()
        {
            for (var i = 0; i < racks.Count; i++)
            {
                var go = new GameObject("Connection_" + (i + 1));
                go.transform.SetParent(InfrastructureRoot, false);
                var line = go.AddComponent<LineRenderer>();
                line.positionCount = 2;
                var rackPos = racks[i].transform.position + Vector3.up * 0.13f;
                line.SetPosition(0, rackPos);
                line.SetPosition(1, SwiftTerminal.position + Vector3.up * 0.06f);
                line.startWidth = 0.006f;
                line.endWidth = 0.006f;
                var material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.Cyan, 0.35f), true);
                line.material = material;
                connections.Add(line);
                connectionMaterials.Add(material);
            }
        }

        private static void EnsureCameraAndLight()
        {
            if (XRSettings.isDeviceActive) EnsureXrRig();
            else EnsureDebugCamera();

            if (Object.FindAnyObjectByType<Light>() == null)
            {
                var lightGo = new GameObject("Andes_KeyLight");
                var light = lightGo.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.1f;
                light.color = AndesPalette.TextBlueWhite;
                lightGo.transform.rotation = Quaternion.Euler(48f, -28f, 0f);
            }
        }

        private static void EnsureDebugCamera()
        {
            Camera camera;
            if (Camera.main == null)
            {
                var cameraGo = new GameObject("Andes_DebugCamera");
                cameraGo.tag = "MainCamera";
                camera = cameraGo.AddComponent<Camera>();
                cameraGo.AddComponent<AudioListener>();
            }
            else
            {
                camera = Camera.main;
            }

            camera.transform.position = ViewerPosition;
            camera.transform.LookAt(PanelFocus);
            ConfigureMainCamera(camera);
        }

        private static void EnsureXrRig()
        {
            var rig = Object.FindAnyObjectByType<XROrigin>();
            Camera camera;
            if (rig == null)
            {
                var rigGo = new GameObject("ANDES_SIM_XROrigin");
                rigGo.SetActive(false);
                rig = rigGo.AddComponent<XROrigin>();
                var offset = new GameObject("Camera Offset");
                offset.transform.SetParent(rig.transform, false);
                rig.CameraFloorOffsetObject = offset;
                var cameraGo = new GameObject("XR Main Camera", typeof(Camera), typeof(AudioListener));
                cameraGo.transform.SetParent(offset.transform, false);
                camera = cameraGo.GetComponent<Camera>();
                cameraGo.AddComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>();
                rig.Camera = camera;
                rigGo.SetActive(true);
            }
            else
            {
                camera = rig.Camera;
                if (camera == null)
                {
                    var offset = rig.CameraFloorOffsetObject;
                    if (offset == null)
                    {
                        offset = new GameObject("Camera Offset");
                        offset.transform.SetParent(rig.transform, false);
                        rig.CameraFloorOffsetObject = offset;
                    }
                    var cameraGo = new GameObject("XR Main Camera", typeof(Camera), typeof(AudioListener));
                    cameraGo.transform.SetParent(offset.transform, false);
                    camera = cameraGo.GetComponent<Camera>();
                    rig.Camera = camera;
                }
                if (camera.GetComponent<AudioListener>() == null)
                    camera.gameObject.AddComponent<AudioListener>();
                if (camera.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>() == null)
                    camera.gameObject.AddComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>();
            }

            rig.transform.position = new Vector3(0f, 0f, ViewerPosition.z);
            rig.transform.rotation = Quaternion.Euler(-6f, 0f, 0f);
            rig.CameraYOffset = ViewerPosition.y;
            if (rig.CameraFloorOffsetObject != null)
                rig.CameraFloorOffsetObject.transform.localPosition = new Vector3(0f, ViewerPosition.y, 0f);
            rig.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;

            ConfigureMainCamera(camera);
        }

        private static void ConfigureMainCamera(Camera camera)
        {
            camera.enabled = true;
            camera.fieldOfView = 58f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = AndesPalette.Alpha(AndesPalette.DeepBlueSolid, 1f);
            camera.gameObject.tag = "MainCamera";

            foreach (var other in Object.FindObjectsByType<Camera>())
            {
                if (other == camera) continue;
                other.enabled = false;
                if (other.CompareTag("MainCamera")) other.gameObject.tag = "Untagged";
            }
        }

        public IEnumerator Materialize()
        {
            var root = InfrastructureRoot;
            var original = root.localScale;
            root.localScale = original * 0.01f;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 1.7f)
            {
                root.localScale = Vector3.Lerp(original * 0.01f, original, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
            root.localScale = original;
        }

        public void PulseRacks()
        {
            foreach (var rack in racks)
            {
                if (rack == null || HologramAnimationManager.Instance == null) continue;
                AndesCoroutines.Run(HologramAnimationManager.Instance.PulseColor(
                    rack, MaterialFactory.GetColor(rack.material), AndesPalette.Cyan, 1.4f, 2));
            }
        }

        public void IlluminateConnections()
        {
            AndesCoroutines.Run(ConnectionRoutine());
        }

        private IEnumerator ConnectionRoutine()
        {
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 1.6f)
            {
                var alpha = Mathf.Lerp(0.25f, 1f, Mathf.Sin(t * Mathf.PI));
                for (var i = 0; i < connectionMaterials.Count; i++)
                    MaterialFactory.SetColor(connectionMaterials[i], AndesPalette.Alpha(AndesPalette.Cyan, alpha));
                yield return null;
            }
            for (var i = 0; i < connectionMaterials.Count; i++)
                MaterialFactory.SetColor(connectionMaterials[i], AndesPalette.Alpha(AndesPalette.Cyan, 0.35f));
        }

        public void PulseTerminal()
        {
            if (terminalRenderer == null || HologramAnimationManager.Instance == null) return;
            AndesCoroutines.Run(HologramAnimationManager.Instance.PulseColor(
                terminalRenderer, MaterialFactory.GetColor(terminalRenderer.material), AndesPalette.ElectricBlue, 1.3f, 2));
        }

        public IEnumerator PrinterFeed()
        {
            if (paper == null) yield break;
            paper.gameObject.SetActive(true);
            paper.localPosition = paperRest;
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 0.9f)
            {
                paper.localPosition = paperRest + Vector3.forward * Mathf.Lerp(0f, 0.09f, t);
                BlinkPrinterIndicator(t);
                yield return null;
            }
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 0.5f)
            {
                paper.localPosition = paperRest + Vector3.forward * Mathf.Lerp(0.09f, 0f, t);
                yield return null;
            }
            paper.gameObject.SetActive(false);
            if (printerIndicator != null)
                MaterialFactory.SetColor(printerIndicator.material, AndesPalette.Alpha(AndesPalette.Cyan, 0.9f));
        }

        private void BlinkPrinterIndicator(float t)
        {
            if (printerIndicator == null) return;
            var on = Mathf.Sin(t * Mathf.PI * 8f) > 0f;
            MaterialFactory.SetColor(printerIndicator.material,
                on ? AndesPalette.Alpha(AndesPalette.Amber, 1f) : AndesPalette.Alpha(AndesPalette.Amber, 0.15f));
        }

        public IEnumerator ScanRacks()
        {
            if (HologramAnimationManager.Instance == null) yield break;
            yield return HologramAnimationManager.Instance.ScanArea(
                new Vector3(0f, 0.88f, 0f), new Vector3(1.2f, 0.3f, 0.9f), AndesPalette.Cyan, 1.8f);
        }

        public IEnumerator TraceAccessLine()
        {
            var tracer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            tracer.name = "Access_Tracer";
            Destroy(tracer.GetComponent<Collider>());
            tracer.transform.localScale = Vector3.one * 0.018f;
            tracer.GetComponent<Renderer>().material = MaterialFactory.Create(AndesPalette.Alpha(AndesPalette.Amber, 1f), true);
            var start = SwiftTerminal.position + Vector3.up * 0.06f;
            var end = new Vector3(-0.13f, 1.01f, 0f);
            for (var t = 0f; t < 1f; t += Time.unscaledDeltaTime / 1.5f)
            {
                tracer.transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }
            Destroy(tracer);
        }
    }
}
