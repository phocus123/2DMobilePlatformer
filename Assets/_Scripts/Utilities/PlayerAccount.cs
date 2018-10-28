using UnityEngine;

namespace PHOCUS.Utilities
{
    public class PlayerAccount : Singleton<PlayerAccount>
    {
        [Header("User")]
        public string DisplayName;
        public int AccountLevel;
        [Header("Currency")]
        public int Coins;
        public int CashPoints;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
        }


    }
}