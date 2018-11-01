using PHOCUS.UI;
using PHOCUS.Utilities;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabCurrencyManager : Singleton<PlayFabCurrencyManager>
{
    public MenuUI MenuUI;

    const string LIVES_CODE = "LV";
    const string COINS_CODE = "GC";
    DateTime nextFreeLife = new DateTime();
    bool livesCapped = true;

    public delegate void CurrencyDelegate(string lives, string coins);
    public event CurrencyDelegate OnReceivedCurrency;
    public Action<string> OnReceivedRechargeTime = delegate { };
    public Action<bool> OnPlayerHasLives = delegate { };
 
    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        GetCurrencyValues();
        MenuUI.OnNextFreeLifeClicked += GetNextFreeLifeTime;
	}

    public void SubtractLife()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest()
        {
            FunctionName = "SubtractLife"
        };

        PlayFabClientAPI.ExecuteCloudScript(request, SubtractLifeCallback, OnApiCallError);
    }

    public void CheckLives()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest()
        {
            FunctionName = "CheckLives"
        };

        PlayFabClientAPI.ExecuteCloudScript(request, CheckLivesCallback, OnApiCallError);
    }

    public void AddCoins(int coins)
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest()
        {
            FunctionName = "AddCoins",
            FunctionParameter = new { amount = coins }
        };

        PlayFabClientAPI.ExecuteCloudScript(request, AddCoinsCallback, OnApiCallError);
    }

    void GetCurrencyValues()
    {
        GetUserInventoryRequest request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, GetCurrencyCallback, OnApiCallError);
    }

    void GetNextFreeLifeTime()
    {
        GetUserInventoryRequest request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, FreeLifeTimeCallback, OnApiCallError);
    }

    void GetCurrencyCallback(GetUserInventoryResult result)
    {
        int livesBalance;
        int coinsBalance;

        result.VirtualCurrency.TryGetValue(LIVES_CODE, out livesBalance);
        result.VirtualCurrency.TryGetValue(COINS_CODE, out coinsBalance);
        OnReceivedCurrency(livesBalance.ToString(), coinsBalance.ToString());
    }

    void CheckLivesCallback(ExecuteCloudScriptResult result)
    {
        if (result.FunctionResult == null)
            OnPlayerHasLives(true);
        else
        {
            OnPlayerHasLives(false);
        }
    }

    void FreeLifeTimeCallback(GetUserInventoryResult result)
    {
        VirtualCurrencyRechargeTime rechargeDetails;
        int livesBalance;
        result.VirtualCurrency.TryGetValue(LIVES_CODE, out livesBalance);

        if (result.VirtualCurrencyRechargeTimes.TryGetValue(LIVES_CODE, out rechargeDetails))
        {
            string timeText = string.Empty;
            if (livesBalance < rechargeDetails.RechargeMax)
            {
                nextFreeLife = DateTime.Now.AddSeconds(rechargeDetails.SecondsToRecharge);

                string minutes = Mathf.Floor(rechargeDetails.SecondsToRecharge / 60).ToString("00");
                string seconds = (rechargeDetails.SecondsToRecharge % 60).ToString("00");
                timeText = string.Format("{0}:{1}", minutes, seconds);

                OnReceivedRechargeTime(timeText);
                livesCapped = false;
            }
            else
            {
                timeText = "You are at the max amount.";
                OnReceivedRechargeTime(string.Empty);
                livesCapped = true;
            }
        }
    }

    void SubtractLifeCallback(ExecuteCloudScriptResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(string.Format("{0} -- {1}", result.Error, result.Error.Message));
            return;
        }

        GetCurrencyValues();
    }

    void AddCoinsCallback(ExecuteCloudScriptResult result)
    {
        if (result.Error != null)
        {
            Debug.LogError(string.Format("{0} -- {1}", result.Error, result.Error.Message));
            return;
        }

        GetCurrencyValues();
    }

    void OnApiCallError(PlayFabError err)
    {
        string http = string.Format("HTTP:{0}", err.HttpCode);
        string message = string.Format("ERROR:{0} -- {1}", err.Error, err.ErrorMessage);
        string details = string.Empty;

        if (err.ErrorDetails != null)
        {
            foreach (var detail in err.ErrorDetails)
            {
                details += string.Format("{0} \n", detail.ToString());
            }
        }

        Debug.LogError(string.Format("{0}\n {1}\n {2}\n", http, message, details));
    }
}
