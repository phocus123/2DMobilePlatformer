using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PHOCUS.Utilities;
using System.Collections;

namespace PHOCUS.UI
{
    public struct RegisterDetails
    {
        public string Email;
        public string Password;
        public string ConfirmPassword;
        public string Username;

        public RegisterDetails(string email, string password, string confirmPassword, string username)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            Username = username;
        }
    }

    public struct LoginDetails
    {
        public string Email;
        public string Password;

        public LoginDetails(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class LoginUI : MonoBehaviour
    {
        [Header("Register")]
        public PlayFabRegister Register;
        public CanvasGroup RegisterAlertCanvas;
        public TextMeshProUGUI RegisterAlertText;
        public CanvasGroup RegisterCanvas;
        public TMP_InputField EmailInput;
        public TMP_InputField PasswordInput;
        public TMP_InputField ConfirmPasswordInput;
        public TMP_InputField UsernameInput;
        public Button RegisterButton;
        public Button CancelButton;
        [Header("Login")]
        public PlayFabLogin Login;
        public CanvasGroup LoginCanvas;
        public CanvasGroup LoginAlertCanvas;
        public TextMeshProUGUI LoginAlertText;
        public TMP_InputField LoginEmailInput;
        public TMP_InputField LoginPasswordInput;
        public Button LoginRegisterButton;
        public Button LoginButton;
    
        public Action<RegisterDetails> OnRegister = delegate { };
        public Action<LoginDetails> OnLogin = delegate { };

        void Awake()
        {
            RegisterButton.onClick.AddListener(OnRegisterClicked);
            CancelButton.onClick.AddListener(OnCancelClicked);
            LoginButton.onClick.AddListener(OnLoginClicked);
            LoginRegisterButton.onClick.AddListener(OnLoginRegisterClicked);

            Login.OnLoginFailure += OnLoginFailure;
            Register.OnRegisterFailure += OnRegisterFailure;
            Register.OnRegisterSuccess += OnRegisterSuccess;
        }

        void OnRegisterClicked()
        {
            RegisterDetails registerDetails = new RegisterDetails(EmailInput.text, PasswordInput.text, ConfirmPasswordInput.text, UsernameInput.text);
            OnRegister(registerDetails);
        }

        void OnLoginClicked()
        {
            LoginDetails loginDetails = new LoginDetails(LoginEmailInput.text, LoginPasswordInput.text);
            OnLogin(loginDetails);
        }

        void OnCancelClicked()
        {
            RegisterCanvas.Toggle();
            LoginCanvas.Toggle();
        }

        void OnLoginRegisterClicked()
        {
            LoginCanvas.Toggle();
            RegisterCanvas.Toggle();
        }

        void OnLoginFailure(string error)
        {
            SetAlertText(LoginAlertText, LoginAlertCanvas, error);
        }

        void OnRegisterFailure(string error)
        {
            SetAlertText(RegisterAlertText, RegisterAlertCanvas, error);
        }

        void OnRegisterSuccess(string text)
        {
            SetAlertText(RegisterAlertText, RegisterAlertCanvas, text);
            StartCoroutine(DelayBeforeCanvasToggle());
        }

        void SetAlertText(TextMeshProUGUI tmpUI, CanvasGroup textCanvas, string text)
        {
            textCanvas.alpha = 1;
            tmpUI.text = text;
        }

        IEnumerator DelayBeforeCanvasToggle()
        {
            yield return new WaitForSeconds(2f);
            OnCancelClicked();
        }
    }
}