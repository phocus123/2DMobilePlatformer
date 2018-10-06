using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PHOCUS.UI
{
    public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Action<bool> OnMouseOverButton = delegate { };

        bool mouseOverButton;

        public bool MouseOverButton
        {
            get { return mouseOverButton; }
            set
            {
                mouseOverButton = value;
                OnMouseOverButton(mouseOverButton);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            MouseOverButton = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MouseOverButton = false;
        }
    }
}