using PHOCUS.Character;
using UnityEngine;

namespace PHOCUS.Utilities
{
    public class BackgroundScroller : MonoBehaviour
    {
        public Player player;
        public float xVelocity, yVelocity;

        Material material;
        Vector2 offset;

        void Awake()
        {
            material = GetComponent<Renderer>().material;
        }

        void Update()
        {
            offset = new Vector2(xVelocity, yVelocity);
            material.mainTextureOffset += offset * Time.deltaTime;
        }

    }
}