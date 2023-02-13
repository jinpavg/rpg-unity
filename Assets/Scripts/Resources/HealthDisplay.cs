using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RPG.Core;
using UnityEngine.UI;

// alert: eventually, I want to move Health into resources once I implement stats
namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        private void Awake() {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0}%", health.GetPercentage());
        }
    }

}