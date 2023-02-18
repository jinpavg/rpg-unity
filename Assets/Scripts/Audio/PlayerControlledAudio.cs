using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;

namespace RPG.Audio
{
    public class PlayerControlledAudio : MonoBehaviour
    {
        const int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        AlkarraSoundHelper helper;
        GameObject player;

        private void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            player = GameObject.FindWithTag("Player");

            FootfallOn(1f);
            FootfallRate(140f);

            // helper.ParameterChangedEvent += (object sender, AlkarraSoundHelper.ParameterChangedEventArgs args) => {
            //     Debug.LogFormat("got param change {0} {1} {2}", args.Index, args.Value, args.Time);
            // };
            // helper.MessageEvent += (object sender, AlkarraSoundHelper.MessageEventArgs args) => {
            //     Debug.LogFormat("got message {0} [{1}] {2}", args.Tag, string.Join(", ", args.Values), args.Time);
            // };
        }

        private void Update()
        {

            Vector3 playerVelocity = player.GetComponent<NavMeshAgent>().velocity;

            if (playerVelocity.magnitude > 0)
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
            helper.SetParamValue(18, isMoving);
        }

        public void FootfallGain(float gain)
        {
            helper.SetParamValue(23, gain);
        }

        public void FootfallRate(float rate)
        {
            helper.SetParamValue(20, rate);
        }

    }

}
