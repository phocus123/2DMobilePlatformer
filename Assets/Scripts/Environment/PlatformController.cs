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
        public bool TriggerCompletion; // TODO For debugging, remove when no longer needed.
        public bool IsPlatformActive;
        public bool IsPlatformCompleted;
        public float CountdownTime;
        public float TimeBetweenSpawn = 2f;
        public Transform SpawnPoint;
        public Portal Portal;
        public GameObject shopkeeperPrefab;
        [Header("Enemy Spawn Pattern")]
        public SpawnPattern[] SpawnPatterns;
        [Header("Paths")]
        public PathController[] Paths;

        Dialogue dialogue;
        List<Enemy> enemiesSpawned = new List<Enemy>();
        bool isReadyToSpawn;
        bool countdownTriggered;
        bool hasSpawned;
        bool isFinalEnemy;
        bool completionTriggered; // TODO For debugging, remove when no longer needed.
        int spawnPatternIndex = 0;
        int spawnGroupIndex = 0;

        void Awake()
        {
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
            CheckStartCountdown();
            CheckTriggerCompletion();
        }

        public void ResetPlatform()
        {
            countdownTriggered = false;
            hasSpawned = false;
            IsPlatformCompleted = false;
            isFinalEnemy = false;
            TriggerCompletion = false;
            completionTriggered = false;
            spawnPatternIndex = 0;
            spawnGroupIndex = 0;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                IsPlatformActive = true;
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                IsPlatformActive = true;
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                IsPlatformActive = false;    
        }

        void CheckTriggerCompletion() // TODO For debugging, remove when no longer needed.
        {
            if (TriggerCompletion && !completionTriggered)
            {
                completionTriggered = true;
                spawnPatternIndex = SpawnPatterns.Length -1;
                spawnGroupIndex = SpawnPatterns[spawnPatternIndex].SpawnGroup.Length - 1;

                foreach (Enemy enemy in enemiesSpawned)
                {
                    Destroy(enemy.gameObject);
                }

                enemiesSpawned.Clear();
                hasSpawned = true;
                isReadyToSpawn = false;
                TriggerPlatformComplete();
                TriggerCompletion = false;
                completionTriggered = false;
            }
        }

        void CheckStartCountdown()
        {
            if (IsPlatformActive && !countdownTriggered && !IsPlatformCompleted)
            {
                countdownTriggered = true;
                StartCoroutine(SpawnCountdown());
            }
        }

        IEnumerator SpawnCountdown()
        {
            float timer = 0f;

            while (timer < CountdownTime)
            {
                timer += Time.deltaTime;
                float timeRemaining = CountdownTime - timer;
                string time = string.Format("Enemies spawning in {0} seconds", Mathf.Round(timeRemaining));
                UIManager.Instance.SetAlertText(time);
                yield return null;
            }

            UIManager.Instance.SetAlertText(string.Empty);
            isReadyToSpawn = true;
        }

        IEnumerator SpawnEnemyOverTime()
        {
            isReadyToSpawn = false;
            string message = string.Format("Spawning wave: {0}/{1}", spawnPatternIndex + 1, SpawnPatterns.Length);
            UIManager.Instance.SetAndFadeAlertText(message);

            for (spawnGroupIndex = 0; spawnGroupIndex < SpawnPatterns[spawnPatternIndex].SpawnGroup.Length; spawnGroupIndex++)
            {
                var enemy = Instantiate(SpawnPatterns[spawnPatternIndex].SpawnGroup[spawnGroupIndex], SpawnPoint.position, Quaternion.identity, SpawnPoint.transform).GetComponent<Enemy>();
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
            GameObject go = Instantiate(shopkeeperPrefab, SpawnPoint.position, Quaternion.identity);
            Shopkeeper shopKeeper = go.GetComponent<Shopkeeper>();
            shopKeeper.Platform = this;
            shopKeeper.SpawnShopkeeper();
        }

        void HandleEnemies()
        {
            if (!IsPlatformCompleted && isReadyToSpawn && !hasSpawned && spawnPatternIndex < SpawnPatterns.Length)
            {
                Portal.GetComponentInChildren<Animator>().SetBool("Animate", true);
                StartCoroutine(SpawnEnemyOverTime());
            }
        }

        void CheckFinalEnemy()
        {
            if(spawnPatternIndex == SpawnPatterns.Length - 1)
                if(spawnGroupIndex == SpawnPatterns[spawnPatternIndex].SpawnGroup.Length -1)
                    isFinalEnemy = true;
        }

        void UpdateEnemy(Enemy enemy)
        {
            if (isFinalEnemy && enemiesSpawned.Count == 1)
                TriggerPlatformComplete();

            if (enemiesSpawned.Count == 1 && spawnPatternIndex < SpawnPatterns.Length - 1)
            {
                spawnPatternIndex = Mathf.Clamp(spawnPatternIndex, 0, SpawnPatterns.Length - 1);
                spawnPatternIndex++;
                isReadyToSpawn = true;
                hasSpawned = false;
            }
            
            enemy.OnEnemyDeath -= UpdateEnemy;
            enemiesSpawned.Remove(enemy);
        }

        void TriggerPlatformComplete()
        {
            IsPlatformCompleted = true;
            Portal.GetComponentInChildren<Animator>().SetBool("Animate", false);
            UIManager.Instance.SetAndFadeAlertText("Platform Completed");
            StartCoroutine(SpawnShopkeeper());
        }
    }
}   
