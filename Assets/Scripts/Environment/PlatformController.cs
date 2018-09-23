using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHOCUS.Environment
{
    public class PlatformController : MonoBehaviour
    {
        public float CountdownTime = 10f;
        public Transform SpawnPoint;
        public GameObject enemy;

        [HideInInspector] public Platform[] Platforms;
        public Portal Portal;

        public bool isReadyToSpawn;

        bool hasSpawned;

        void Start()
        {
            Platforms = GetComponentsInChildren<Platform>();
            Portal = GetComponentInChildren<Portal>();
        }

        void Update()
        {
            if (isReadyToSpawn && !hasSpawned)
            {
                Portal.GetComponentInChildren<Animator>().SetBool("Animate", true);
                Instantiate(enemy, SpawnPoint.position, Quaternion.identity, SpawnPoint.transform);
                hasSpawned = true;
            }
        }
    }   
}