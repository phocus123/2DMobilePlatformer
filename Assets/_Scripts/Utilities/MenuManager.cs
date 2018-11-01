using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PHOCUS.Utilities
{
    public class MenuManager : MonoBehaviour
    {
        void Awake()
        {
            PlayFabCurrencyManager.Instance.OnPlayerHasLives += StartGame;     
        }

        void StartGame(bool start)
        {
            if (start)
                SceneManager.LoadScene(2);
            else
            {
                print("You have no lives");
                // TODO show UI
            }
        }
    }
}