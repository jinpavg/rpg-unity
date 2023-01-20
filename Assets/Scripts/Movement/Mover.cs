using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] Transform target;

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination;
        }

        private void UpdateAnimator()
        {
            // get global velocity from nav mesh agent
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;

            // convert to local value relative to character
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            // pass speed to the animator
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);

        }

    }
}