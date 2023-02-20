using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    
    public class DeathScript : MonoBehaviour 
    
    {
        int sceneIndex;
        
        private void Start() 
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        public int GetSceneIndex()
        {
            return sceneIndex;
        }

    }
}