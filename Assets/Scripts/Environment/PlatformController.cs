﻿using PHOCUS.Character;
using PHOCUS.UI;
using System;
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
        [HideInInspector] public Platform[] Platforms;
        public float CountdownTime = 10f;
        public Transform SpawnPoint;
        public Portal Portal;
        public GameObject shopkeeperPrefab;
        public bool isReadyToSpawn;
        [Header("Enemy Spawn Pattern")]
        public SpawnPattern[] SpawnPatterns;

        List<Enemy> enemiesSpawned = new List<Enemy>();
        bool hasSpawned;
        bool isFinalWave;
        int spawnIndex = 0;

        void Start()
        {
            Platforms = GetComponentsInChildren<Platform>();
            Portal = GetComponentInChildren<Portal>();
        }

        void Update()
        {
            HandleEnemies();
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
            if (isFinalWave)
                TriggerPlatformComplete();

            if (enemiesSpawned.Count == 1 && spawnIndex < SpawnPatterns.Length - 1)
            {
                spawnIndex = Mathf.Clamp(spawnIndex, 0, SpawnPatterns.Length - 1);
                spawnIndex++;
                isReadyToSpawn = true;
                hasSpawned = false;
            }
            
            enemy.OnEnemyDeath -= UpdateEnemy;
            enemiesSpawned.Remove(enemy);

            if (enemiesSpawned.Count == 0 && spawnIndex == SpawnPatterns.Length - 1)
                isFinalWave = true;
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
            string message = string.Format("Spawning wave: {0}/{1}", spawnIndex + 1, SpawnPatterns.Length);
            UIManager.Instance.SetAlertText(message);
            yield return new WaitForSeconds(2f);

            for (int i = 0; i < SpawnPatterns[spawnIndex].SpawnGroup.Length; i++)
            {
                var enemy = Instantiate(SpawnPatterns[spawnIndex].SpawnGroup[i], SpawnPoint.position, Quaternion.identity, SpawnPoint.transform).GetComponent<Enemy>();
                enemiesSpawned.Add(enemy);
                enemy.OnEnemyDeath += UpdateEnemy;
                hasSpawned = true;
                yield return new WaitForSeconds(2f);
            }
        }

        IEnumerator SpawnShopkeeper()
        {
            yield return new WaitForSeconds(1.5f);
            Instantiate(shopkeeperPrefab, SpawnPoint.position, Quaternion.identity);
        }

    }
}   
