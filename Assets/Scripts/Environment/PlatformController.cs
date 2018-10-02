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
        [HideInInspector] public bool isReadyToSpawn;
        public float CountdownTime;
        public float TimeBetweenSpawn = 2f;
        public Transform SpawnPoint;
        public Portal Portal;
        public GameObject shopkeeperPrefab;
        [Header("Platforms")]
        public Platform[] Platforms;
        [Header("Enemy Spawn Pattern")]
        public SpawnPattern[] SpawnPatterns;

        Dialogue dialogue;
        List<Enemy> enemiesSpawned = new List<Enemy>();
        bool hasSpawned;
        bool isFinalEnemy;
        int spawnPatterIndex = 0;
        int spawnGroupIndex = 0;

        void Awake()
        {
            Platforms = GetComponentsInChildren<Platform>();
            Portal = GetComponentInChildren<Portal>();
            dialogue = UIManager.Instance.Dialogue;
        }

        void Start()
        {
            dialogue.OnResetPlatform += ResetPlatform;    
        }

        void Update()
        {
            HandleEnemies();
            CheckFinalEnemy();
        }

        public void ResetPlatform()
        {
            Platforms[0].Reset();

            hasSpawned = false;
            spawnPatterIndex = 0;
        }

        void HandleEnemies()
        {
            if (isReadyToSpawn && !hasSpawned && spawnPatterIndex < SpawnPatterns.Length)
            {
                Portal.GetComponentInChildren<Animator>().SetBool("Animate", true);
                StartCoroutine(SpawnEnemyOverTime());
            }
        }

        void UpdateEnemy(Enemy enemy)
        {
            if (isFinalEnemy && enemiesSpawned.Count == 1)
                TriggerPlatformComplete();

            if (enemiesSpawned.Count == 1 && spawnPatterIndex < SpawnPatterns.Length - 1)
            {
                spawnPatterIndex = Mathf.Clamp(spawnPatterIndex, 0, SpawnPatterns.Length - 1);
                spawnPatterIndex++;
                isReadyToSpawn = true;
                hasSpawned = false;
            }
            
            enemy.OnEnemyDeath -= UpdateEnemy;
            enemiesSpawned.Remove(enemy);
        }

        void CheckFinalEnemy()
        {
            if(spawnPatterIndex == SpawnPatterns.Length - 1)
                if(spawnGroupIndex == SpawnPatterns[spawnPatterIndex].SpawnGroup.Length -1)
                    isFinalEnemy = true;
        }

        void TriggerPlatformComplete()
        {
            Portal.GetComponentInChildren<Animator>().SetBool("Animate", false);
            UIManager.Instance.SetAlertText("Platform Completed");
            StartCoroutine(SpawnShopkeeper());
        }

        IEnumerator SpawnEnemyOverTime()
        {
            isReadyToSpawn = false;
            string message = string.Format("Spawning wave: {0}/{1}", spawnPatterIndex + 1, SpawnPatterns.Length);
            UIManager.Instance.SetAlertText(message);

            for (spawnGroupIndex = 0; spawnGroupIndex < SpawnPatterns[spawnPatterIndex].SpawnGroup.Length; spawnGroupIndex++)
            {
                var enemy = Instantiate(SpawnPatterns[spawnPatterIndex].SpawnGroup[spawnGroupIndex], SpawnPoint.position, Quaternion.identity, SpawnPoint.transform).GetComponent<Enemy>();
                enemiesSpawned.Add(enemy);
                enemy.OnEnemyDeath += UpdateEnemy;
                hasSpawned = true;
                CheckFinalEnemy();
                yield return new WaitForSeconds(TimeBetweenSpawn);
            }
        }

        IEnumerator SpawnShopkeeper()
        {
            yield return new WaitForSeconds(1.5f);
            Instantiate(shopkeeperPrefab, SpawnPoint.position, Quaternion.identity);
        }
    }
}   
