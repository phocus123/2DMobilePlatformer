using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PHOCUS.Environment
{
    public class PathDecorations : MonoBehaviour
    {
        public Tilemap tilemap;

        Color tmp = Color.white;
        float disabledAlpha = 0.2f;

        void Awake()
        {
            if (tilemap == null)
                tilemap = GetComponent<Tilemap>();
        }

        public void ToggleAlpha()
        {
            tmp.a = disabledAlpha;
          
            tilemap.color = tilemap.color == Color.white ? tmp : Color.white;
        }
    }
}