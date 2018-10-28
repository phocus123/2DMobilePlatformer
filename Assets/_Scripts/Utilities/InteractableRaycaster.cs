using System;
using UnityEngine;

namespace PHOCUS.Utilities
{
    public class InteractableRaycaster : MonoBehaviour
    {
        public Action OnInteractableClicked = delegate { };
        public Action<bool> OnMouseOverInteractable = delegate { };

        bool mouseOverInteractable;

        public bool MouseOverInteractable
        {
            get { return mouseOverInteractable; }
            set
            {
                if (mouseOverInteractable == value)
                    return;

                mouseOverInteractable = value;
                OnMouseOverInteractable(mouseOverInteractable);
            }
        }

        void Update()
        {
            LayerMask interactableLayer = 1 << 15;
            float zPlane = 0f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 posAtZ = ray.origin + ray.direction * (zPlane - ray.origin.z) / ray.direction.z;
            Vector2 point = new Vector2(posAtZ.x, posAtZ.y);
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, interactableLayer);

            if (hit.collider != null)
            {
                MouseOverInteractable = true;

                if (Input.GetMouseButtonDown(0))
                    OnInteractableClicked();
            }
            else
                MouseOverInteractable = false;
        }
    }
}