using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Audio;

namespace RPG.Core
{
    
    public class DeathScript : MonoBehaviour 
    
    {
        int sceneIndex;
        const int pluginKey = 1;
        AlkarraSoundHelper helper;
        
        private void Start() 
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            helper = AlkarraSoundHelper.FindById(pluginKey);
            helper.SetParamValue(10, 0); // lower waveshape gain if it happens to be up
        }

        public int GetSceneIndex()
        {
            return sceneIndex;
        }

    }
}