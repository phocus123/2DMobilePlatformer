using UnityEngine;

namespace PHOCUS.Environment
{
    public class PathController : MonoBehaviour
    {
        public Path[] Paths;
        public Transform CameraLookAt;
        public PathDecorations pathDecorations;
        public int GemCost;
        public bool PathsEnabled;

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

            PathsEnabled = !PathsEnabled;
            pathDecorations.ToggleAlpha();
        }
    }
}