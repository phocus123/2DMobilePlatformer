using UnityEngine;

namespace PHOCUS.Character
{
    [CreateAssetMenu(menuName = "Player Stats/Attack Stat")]
    public class AttackStat : PlayerStat
    {
        public int Attack01Damage;
        public int Attack02Damage;
        public int Attack03Damage;
    }
}