using UnityEngine;

namespace AndesSim
{
    // Hace que el servidor sea seleccionable con PINCH, POKE o clic en modo prueba.
    public class ServerSelectionTarget : MonoBehaviour, IAndesInteractable
    {
        public bool Selected => manager != null && manager.IsSelected;
        public Vector3 InteractionPoint => transform.position + Vector3.up * 0.08f;
        public float InteractionRadius => 0.13f;
        public bool IsEnabled => !Selected;

        private ServerHighlightManager manager;

        public void Configure(ServerHighlightManager highlightManager)
        {
            manager = highlightManager;
        }

        public void SetHover(bool hover)
        {
            manager?.SetHover(hover);
        }

        public void Select()
        {
            if (Selected) return;
            manager?.Select();
        }

        private void Start()
        {
            HandInteractionManager.Instance?.Register(this);
        }

        private void OnDestroy()
        {
            HandInteractionManager.Instance?.Unregister(this);
        }
    }
}
