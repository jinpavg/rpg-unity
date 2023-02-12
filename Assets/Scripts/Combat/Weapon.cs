using UnityEngine;

namespace RPG.Combat
{
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;

        public void Spawn(Transform handTransform, Animator animator)
            {
                if (equippedPrefab != null)
                {
                    Instantiate(equippedPrefab, handTransform);
                }
                if (animatorOverride != null)
                {
                    animator.runtimeAnimatorController = animatorOverride;
                }
            }

        public float GetRange()
        {
            return weaponRange;
        }

        public float GetDamage()
        {
            return weaponDamage;
        }
    }
}