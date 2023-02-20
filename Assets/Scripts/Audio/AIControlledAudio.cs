using UnityEngine;
using UnityEngine.AI;

namespace RPG.Audio
{
    
    public class AIControlledAudio : MonoBehaviour 
    {
        AudioSource footFallSource;
        bool shouldAllowPlay;

        private void Start() 
        {
            //Fetch the AudioSource from the GameObject
            footFallSource = GetComponent<AudioSource>();

        }

        private void Update()
        {
            Vector3 bugVelocity = GetComponent<NavMeshAgent>().velocity;
            bool isMoving = bugVelocity.magnitude > 0;
            if (isMoving && shouldAllowPlay)
            {
                Debug.Log("bug");
                footFallSource.Play();
                shouldAllowPlay = false;
            }
            else if (!isMoving)
            {
                footFallSource.Stop();
                shouldAllowPlay = true;
            }


        }


    }
}