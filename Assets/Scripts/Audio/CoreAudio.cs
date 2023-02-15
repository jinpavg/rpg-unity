using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class CoreAudio : MonoBehaviour
    {
        int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        AlkarraSoundHelper helper;
        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            helper.SetParamValue(0, 1); // turning on karplusMetroOn
            helper.SetParamValue(1, 400); // setting initial metro
            //helper.SetParamValue(20, 1); // setting delayL to 1 (overriden by bulb)
            //helper.SetParamValue(22, 1); // setting delayR to 1 (overriden by bulb)
            helper.SetParamValue(4, 1); // stringsync

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


