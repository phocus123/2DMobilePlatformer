using PHOCUS.Environment;
using UnityEngine;

namespace PHOCUS.Character
{
    [CreateAssetMenu(menuName = "Items/Path Item")]
    public class PathItem : Item
    {
        public PathController PathController;
    }
}