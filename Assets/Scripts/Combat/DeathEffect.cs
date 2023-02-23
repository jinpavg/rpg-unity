using UnityEngine;

namespace RPG.Combat
{
    
    public class DeathEffect : MonoBehaviour 
    {
        [SerializeField]GameObject deathEffectPrefab = null;

        public void PlayDeathEffect()
        {
            if (deathEffectPrefab == null) return;
            Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        }
    }
}