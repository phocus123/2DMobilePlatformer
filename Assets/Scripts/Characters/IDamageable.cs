using System;

namespace PHOCUS.Character
{
    public interface IDamageable 
    {
        float Health { get; set; }
        void DealDamage(float damageAmount);
    }
}