using UnityEngine;
using RPG.Control;

namespace RPG.Core
{
    
    
    public class VictoryTrigger : MonoBehaviour 
    {
        [SerializeField] GameObject victoryScreenPrefab = null;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player")
            {
                Instantiate(victoryScreenPrefab);
                GetComponent<VictoryControlRemover>().DisableControl();
            }
            
        }
    }
}