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
            // this is a total hack, we should not be checking for this every frame
            if (Vector3.Distance(player.transform.position, transform.position) < 10f)
            {
                helper.SetParamValue(20, 100); // delayL
                helper.SetParamValue(22, 50); // delayR
            }
            else 
            {
                helper.SetParamValue(20, 200); // delayL
                helper.SetParamValue(22, 400); // delayR
            }
        }
    }
}


