using UnityEngine;


namespace RPG.Audio
{
    public class ProjectileAudio : MonoBehaviour
    {
        AudioSource projectileSource;

        private void Start() 
        {
            //Fetch the AudioSource from the GameObject
            projectileSource = GetComponent<AudioSource>();
            projectileSource.Play();

        }

    }
}


