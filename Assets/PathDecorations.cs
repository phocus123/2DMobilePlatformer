using UnityEngine;
using UnityEngine.Tilemaps;

namespace PHOCUS.Environment
{
    public class PathDecorations : MonoBehaviour
    {
        public Tilemap tilemap;

        const float enabledAlpha = 255;
        const float disabledAlpha = 80;

        Color tmp;
        bool isEnabled;
        Color colourOne = new Color(255, 255, 255, 255);
        Color colourTwo = new Color(255, 255, 255, 80);

        void OnValidate()
        {
            if (tilemap == null)
                tilemap = GetComponent<Tilemap>();
        }

        void Awake()
        {
            if (tilemap == null)
                tilemap = GetComponent<Tilemap>();
        }

        public void ToggleAlpha()
        {
            isEnabled = !isEnabled;

            if (isEnabled)
                tilemap.color = colourOne;
            else
                tilemap.color = colourTwo;

        }
    }
}