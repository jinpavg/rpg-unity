using UnityEngine;
using RPG.Saving;
using RPG.Control;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        // temporary maxHealthPoints to be rewritten by stats
        [SerializeField] float maxHealthPoints = 100f;

        bool isDead = false;
        
        // temporary way to aggrevate enemy
        AIController enemyController = null;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            // temporary way to aggrevate enemy
            enemyController = GetComponent<AIController>();

            if (enemyController != null)
            {
                enemyController.Aggrevate();
            }
    
            if (healthPoints == 0)
            {
                Die();
            }
        }

        // will be written once stats exist
        public float GetPercentage()
        {
            return 100 * healthPoints / maxHealthPoints;
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0)
            {
                Die();
            }
        }

    }
}