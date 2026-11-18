using UnityEngine;

namespace AndesSim
{
    // Contrato único para botones, paneles y componentes 3D seleccionables con manos o ratón.
    public interface IAndesInteractable
    {
        Vector3 InteractionPoint { get; }
        float InteractionRadius { get; }
        bool IsEnabled { get; }
        void SetHover(bool hover);
        void Select();
    }
}
