using PHOCUS.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PHOCUS.Utilities
{
    public class PlayFabLogin : MonoBehaviour
    {
        public LoginUI LoginUI;
        public PlayerAccount PlayerAccount;

        public Action<string> OnLoginFailure = delegate { };

        void Awake()
        {
            LoginUI.OnLogin += Login;
        }

        void Login(LoginDetails loginDetails)
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = loginDetails.Email,
                Password = loginDetails.Password
            };

            PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFailure);
        }

        void LoginSuccess(LoginResult result)
        {
            GetPlayerProfile(result.PlayFabId);
        }

        void LoginFailure(PlayFabError error)
        {
            OnLoginFailure(error.ErrorMessage);
        }

        void GetPlayerProfile(string playfabID)
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
            {
                PlayFabId = playfabID
            }, 
            result => 
            {
                SetPlayerAccount(result);
            }, 
            error =>{

            });
        }

        void SetPlayerAccount(GetPlayerProfileResult result)
        {
            PlayerAccount.DisplayName = result.PlayerProfile.DisplayName;
            LoadMenuScene();
        }

        void LoadMenuScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}