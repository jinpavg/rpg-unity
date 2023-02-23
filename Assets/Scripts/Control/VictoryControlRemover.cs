using UnityEngine;
using RPG.Core;

namespace RPG.Control
{
    
    public class VictoryControlRemover : MonoBehaviour 
    {
        GameObject[] enemies;

        private void Start() 
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }

        public void DisableControl()
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<ActionScheduler>().CancelCurrentAction();
                enemy.GetComponent<AIController>().enabled = false;
            }
            
        }
 
        public void EnableControl()
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<AIController>().enabled = true;
            }
        }
    }
}