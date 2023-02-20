using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Audio
{
    public class CoreAudio : MonoBehaviour
    {
        const int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        const int orbsKey = 2;
        AlkarraSoundHelper helper;
        AlkarraOrbsHelper orbsHelper;
        public AudioClip background;
        public AudioClip loadingScreen;
        System.UInt32 backgroundStartInport;
        //System.UInt32 backgroundStopInport;
        System.UInt32 loadingScreenStartInport;
        //System.UInt32 loadingScreenStopInport;

        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            orbsHelper = AlkarraOrbsHelper.FindById(orbsKey);

            helper.SetParamValue(3, 0); // turning off karplusMetroOn
            helper.SetParamValue(4, 400); // setting initial metro
            //helper.SetParamValue(20, 1); // setting delayL to 1 (overriden by bulb)
            //helper.SetParamValue(22, 1); // setting delayR to 1 (overriden by bulb)
            helper.SetParamValue(10, 0); // start with no waveshape
            helper.SetParamValue(8, 1); // stringsync
            helper.SetParamValue(15, 1); // rotate
            helper.SetParamValue(21, 1); // loop sample
            helper.SetParamValue(25, 1); // loop sampleTwo
            helper.SetParamValue(19, 1); // start sample playback rate at 1

            orbsHelper.SetParamValue(5, 0); // start with no orbs

            if (background)
            {
                float[] samples = new float[background.samples * background.channels];
                background.GetData(samples, 0);
                helper.LoadDataRef("sample", samples, background.channels, background.frequency);
            }

            if (loadingScreen)
            {
                float[] samples = new float[loadingScreen.samples * loadingScreen.channels];
                loadingScreen.GetData(samples, 0);
                helper.LoadDataRef("sampleTwo", samples, loadingScreen.channels, loadingScreen.frequency);
            }

            backgroundStartInport = AlkarraSoundHelper.Tag("playSample");
            //backgroundStopInport = AlkarraSoundHelper.Tag("stopSample");
            loadingScreenStartInport = AlkarraSoundHelper.Tag("playSampleTwo");
            //loadingScreenStopInport = AlkarraSoundHelper.Tag("stopSampleTwo");

            helper.SetParamValue(22, 0); // we set the starting gain for background ...
            helper.SetParamValue(26, 0); // ... and loading screen to 0, so they can be raised elsewhere

            helper.SendMessage(backgroundStartInport, 1); // we start both the background ...
            helper.SendMessage(loadingScreenStartInport, 1); // ... and the title theme

            // myInport = AlkarraSoundHelper.Tag("playSample");
            // helper.SendMessage(myInport, 1);

        }

        
    }
}


