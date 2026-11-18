using UnityEngine;

namespace AndesSim
{
    // Punto de entrada automático: construye todos los sistemas y la escena de demostración en Play.
    public class AndesSimBootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Boot()
        {
            if (Object.FindAnyObjectByType<AndesSimBootstrap>() != null) return;
            var go = new GameObject("ANDES_SIM_Bootstrap");
            go.AddComponent<AndesSimBootstrap>();
        }

        private void Start()
        {
            var systems = new GameObject("ANDES_SIM_Systems").transform;
            systems.SetParent(transform, false);

            systems.gameObject.AddComponent<HologramAnimationManager>();
            systems.gameObject.AddComponent<HandInteractionManager>();
            systems.gameObject.AddComponent<AudioManager>();

            var infrastructure = systems.gameObject.AddComponent<InfrastructureBuilder>();
            infrastructure.Build();

            var sequence = systems.gameObject.AddComponent<SceneSequenceManager>();
            sequence.StartSequence(infrastructure);
        }
    }
}
