using UnityEngine;

namespace PHOCUS.Utilities
{
    public class BackgroundFollow : MonoBehaviour
    {
        public float speed = 1f;

        void Update()
        {
            Vector3 newPos = new Vector3(0, Camera.main.transform.position.y, 10f);

            transform.position = Vector3.Lerp(transform.position, newPos, speed);
        }
    }
}