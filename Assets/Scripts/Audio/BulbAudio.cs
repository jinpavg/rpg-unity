using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class BulbAudio : MonoBehaviour
    {
        const int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        AlkarraSoundHelper helper;
        GameObject player;
        bool shouldInteractWithPlayer = false;
        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (!shouldInteractWithPlayer) return;
            float playbackSpeedMultiplicationFactor = Vector3.Distance(player.transform.position, transform.position);
            helper.SetParamValue(19, 1 + (playbackSpeedMultiplicationFactor * 0.1));
        }

        // this doesn't seem to quite do it
        void OnCollisionEnter(Collision other) {
                if (other.gameObject == player)
                {
                    shouldInteractWithPlayer = true; 
                }
                           
        }
        void OnCollisionExit(Collision other) {
            if (other.gameObject == player)
            {
                shouldInteractWithPlayer = false;
            }
            
        
        }
    }
}


