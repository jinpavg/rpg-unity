using UnityEngine;
using RPG.Core;
using RPG.Combat;

namespace RPG.Story
{
    public class VictoryCondition : MonoBehaviour 
    {
        GameObject bigBad;

        private void Start() 
        {
            bigBad = GameObject.FindWithTag("BigBad");    
        }

        private void Update() 
        {
            if (bigBad == null) return;
            if (bigBad.GetComponent<Health>().IsDead())
            {
                GetComponent<DeathEffect>().PlayDeathEffect();
                Destroy(gameObject);
            }
        }
    }
}



