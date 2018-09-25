using PHOCUS.Character;
using PHOCUS.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHOCUS.Environment
{
    [System.Serializable]
    public struct SpawnPattern
    {
        public GameObject[] SpawnGroup;
    }

    public class PlatformController : MonoBehaviour
    {
        public float CountdownTime = 10f;
        public Transform SpawnPoint;
        [HideInInspector] public Platform[] Platforms;
        public Portal Portal;
        public bool isReadyToSpawn;
        [Header("Enemy Spawn Pattern")]
        public SpawnPattern[] SpawnPatterns;

        List<Enemy> enemiesSpawned = new List<Enemy>();
        bool hasSpawned;
        bool platformComplete;
        int spawnIndex = 0;

        void Start()
        {
            Platforms = GetComponentsInChildren<Platform>();
            Portal = GetComponentInChildren<Portal>();
        }

        void Update()
        {
            HandleEnemies();
            CheckPlatformCompletion();
        }

        void HandleEnemies()
        {
            if (isReadyToSpawn && !hasSpawned && spawnIndex < SpawnPatterns.Length)
            {
                Portal.GetComponentInChildren<Animator>().SetBool("Animate", true);

                StartCoroutine(SpawnEnemyOverTime());
            }
        }

        void UpdateEnemy(Enemy enemy)
        {
            if (enemiesSpawned.Count == 1 && spawnIndex < SpawnPatterns.Length - 1)
            {
                spawnIndex = Mathf.Clamp(spawnIndex, 0, SpawnPatterns.Length - 1);
                spawnIndex++;
                isReadyToSpawn = true;
                hasSpawned = false;
            }

            enemy.OnEnemyDeath -= UpdateEnemy;
            enemiesSpawned.Remove(enemy);
        }

        void CheckPlatformCompletion()
        {
            if (!platformComplete)
                if (enemiesSpawned.Count == 0 && spawnIndex == SpawnPatterns.Length - 1)
                {
                    platformComplete = true;
                    Portal.GetComponentInChildren<Animator>().SetBool("Animate", false);
                    UIManager.Instance.SetAlertText("Platform Completed");
                }
        }

        IEnumerator SpawnEnemyOverTime()
        {
            string message = string.Format("Spawning wave: {0}/{1}", spawnIndex + 1, SpawnPatterns.Length);
            UIManager.Instance.SetAlertText(message);

            for (int i = 0; i < SpawnPatterns[spawnIndex].SpawnGroup.Length; i++)
            {
                var enemy = Instantiate(SpawnPatterns[spawnIndex].SpawnGroup[i], SpawnPoint.position, Quaternion.identity, SpawnPoint.transform).GetComponent<Enemy>();
                enemiesSpawned.Add(enemy);
                enemy.OnEnemyDeath += UpdateEnemy;
                hasSpawned = true;
                yield return Wait();
            }
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(2f);
        }
    }
}   
