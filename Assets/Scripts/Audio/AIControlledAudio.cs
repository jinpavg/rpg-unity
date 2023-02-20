using UnityEngine;
using UnityEngine.AI;

namespace RPG.Audio
{
    // this is not currently working and is disabled on the two active enemies
    public class AIControlledAudio : MonoBehaviour 
    {
        const int bugfootKey = 4;
        BugFootHelper bugfootHelper;

        private void Start() 
        {
            bugfootHelper = BugFootHelper.FindById(bugfootKey);
            
            FootfallOn(1f);
            FootfallRate(140f);

        }

        private void Update()
        {

            Vector3 bugVelocity = GetComponent<NavMeshAgent>().velocity;

            if (bugVelocity.magnitude > 0)
            {
                FootfallGain(0.7f);
            }
            else
            {
                FootfallGain(0f);
            }


        }

        public void FootfallOn(float isMoving)
        {
            bugfootHelper.SetParamValue(0, isMoving);
        }

        public void FootfallGain(float gain)
        {
            bugfootHelper.SetParamValue(3, gain);
        }

        public void FootfallRate(float rate)
        {
            bugfootHelper.SetParamValue(1, rate);
        }

    }
}