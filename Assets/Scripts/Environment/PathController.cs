using UnityEngine;

namespace PHOCUS.Environment
{
    public class PathController : MonoBehaviour
    {
        public int GemCost;
        public Path[] Paths;
        public PathDecorations pathDecorations;

        void OnValidate()
        {
            if (pathDecorations == null)
                pathDecorations = GetComponentInChildren<PathDecorations>();
        }

        void Start()
        {
            if (pathDecorations == null)
                pathDecorations = GetComponentInChildren<PathDecorations>();
        }

        public void TogglePaths()
        {
            foreach (Path path in Paths)
            {
                path.IsEnabled = !path.IsEnabled;
            }

            pathDecorations.ToggleAlpha();
        }
    }
}