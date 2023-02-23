using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Audio
{
    public class CinematicAudio : MonoBehaviour
    {
        AlkarraSoundHelper helper;
        const int pluginKey = 1;
        

        private void Start() 
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);

            GetComponent<PlayableDirector>().played += StartKarplus;
            GetComponent<PlayableDirector>().stopped += StopKarplus;
        }

        void StartKarplus(PlayableDirector aDirector)
        {
            helper.SetParamValue(8, 0); // string sync (don't send list of notes)
            helper.SetParamValue(4, 800); // karplus metro 80
            helper.SetParamValue(3, 1); // karplus metro on
        }

        void StopKarplus(PlayableDirector aDirector)
        {
            helper.SetParamValue(3, 0); // karplus metro on
        }

        

        
    }
}

