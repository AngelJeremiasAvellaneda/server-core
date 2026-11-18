using System.Collections;
using UnityEngine;

namespace AndesSim
{
    // Ejecuta corrutinas sin depender de un GameObject de escena concreto.
    public sealed class AndesCoroutines : MonoBehaviour
    {
        private static AndesCoroutines instance;

        public static Coroutine Run(IEnumerator routine)
        {
            Ensure();
            return instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine routine)
        {
            if (instance != null && routine != null)
                instance.StopCoroutine(routine);
        }

        private static void Ensure()
        {
            if (instance != null)
                return;

            var go = new GameObject("ANDES_SIM_CoroutineRunner");
            instance = go.AddComponent<AndesCoroutines>();
        }
    }
}
