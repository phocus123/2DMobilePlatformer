using System.Collections;
using UnityEngine;
namespace PHOCUS.Character
{
    public class Potion : MonoBehaviour
    {
        public enum PotionTypes { Health, Stamina }
        public PotionTypes PotionType;
        public int Value;

        SpriteRenderer parent;
        Vector2 direction;
        InstantiateDirection instantiateDirection = InstantiateDirection.None;
        const float speed = 2.5f;
        bool isTriggered;

        void Start()
        {
            parent = GetComponentInParent<SpriteRenderer>();    
            GetDirection();
            StartCoroutine(Move());
        }

        void GetDirection()
        {
            if (instantiateDirection == InstantiateDirection.None)
            {
                var randomInt = Random.Range(0, 3);
                instantiateDirection = (InstantiateDirection)randomInt;

                switch (instantiateDirection)
                {
                    case InstantiateDirection.UpperCentre:
                        direction = Vector2.up;
                        break;
                    case InstantiateDirection.UpperLeft:
                        direction = (Vector2.left / 2) + Vector2.up;
                        break;
                    case InstantiateDirection.UpperRight:
                        direction = (Vector2.right / 2) + Vector2.up;
                        break;
                }
            }
        }

        IEnumerator Move()
        {
            float progress = 0f;
            float moveLength = 1f;

            while (progress <= moveLength)
            {
                progress += Time.deltaTime;
                float translation = speed * Time.deltaTime;
                parent.transform.Translate(direction * translation);
                yield return null;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && !isTriggered)
            {
                isTriggered = true;
                var player = collision.GetComponent<Player>();

                switch (PotionType)
                {
                    case PotionTypes.Health:
                        player.Health += Value;
                        break;
                    case PotionTypes.Stamina:
                        player.Stamina += Value;
                        break;
                }

                Destroy(parent.gameObject);
            }
        }
    }
}