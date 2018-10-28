using UnityEngine;

namespace PHOCUS.Character
{
    [CreateAssetMenu(menuName = "Player Stats/Stamina Stat")]
    public class StaminaStat : PlayerStat
    {
        public float MaxStamina;
        public float RegenRate;

    }
}