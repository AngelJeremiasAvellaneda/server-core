using UnityEngine;

namespace AndesSim
{
    // Mantiene los paneles legibles orientándolos hacia la cámara (o hacia el usuario en XR).
    public class Billboard : MonoBehaviour
    {
        private Camera target;

        private void LateUpdate()
        {
            if (target == null) target = Camera.main;
            if (target == null) return;
            var toCamera = target.transform.position - transform.position;
            toCamera.y = 0f;
            if (toCamera.sqrMagnitude < 0.0001f) return;
            transform.rotation = Quaternion.LookRotation(-toCamera.normalized, Vector3.up);
        }
    }
}
