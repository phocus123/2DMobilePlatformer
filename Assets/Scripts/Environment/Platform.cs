using System.Collections;
using UnityEngine;
using PHOCUS.UI;

namespace PHOCUS.Environment
{
    public class Platform : MonoBehaviour
    {
        public bool IsTriggered;

        PlatformController parent;

        void Start()
        {
            parent = GetComponentInParent<PlatformController>();
        }

        public void Reset()
        {
            StartCoroutine(SpawnCountdown());
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsTriggered)
            {
                foreach (Platform platform in parent.Platforms)
                {
                    platform.IsTriggered = true;
                }

                StartCoroutine(SpawnCountdown());
            }
        }

        IEnumerator SpawnCountdown()
        {
            if (IsTriggered)
            {
                float timer = 0f;

                while (timer < parent.CountdownTime)
                {
                    timer += Time.deltaTime;
                    float timeRemaining = parent.CountdownTime - timer;
                    string time = string.Format("Enemies spawning in {0} seconds", Mathf.Round(timeRemaining));
                    UIManager.Instance.SetAlertText(time);
                    yield return null;
                }

                UIManager.Instance.SetAlertText(string.Empty);
                parent.isReadyToSpawn = true;
            }
        }
    }
}