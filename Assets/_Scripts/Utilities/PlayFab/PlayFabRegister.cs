using PHOCUS.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

namespace PHOCUS.Utilities
{
    public class PlayFabRegister : MonoBehaviour
    {
        public LoginUI LoginUI;

        public Action<string> OnRegisterFailure = delegate { };
        public Action<string> OnRegisterSuccess = delegate { };

        void Awake()
        {
            LoginUI.OnRegister += RegisterAccount;        
        }

        void RegisterAccount(RegisterDetails registerDetails)
        {
            var request = new RegisterPlayFabUserRequest
            {
                Email = registerDetails.Email, 
                Password = registerDetails.Password,
                Username = registerDetails.Username
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
        }

        void SetAccountDisplayName(string username)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = username.ToUpper()
            },
            result => {
            },
            error => {
                Debug.Log("Error");
            });
        }

        void RegisterSuccess(RegisterPlayFabUserResult result)
        {
            string successText = "Account has been successfully created.";
            OnRegisterSuccess(successText);
            SetAccountDisplayName(result.Username);
        }

        void RegisterFailure(PlayFabError error)
        {
            OnRegisterFailure(error.ErrorMessage);
        }
    }
}