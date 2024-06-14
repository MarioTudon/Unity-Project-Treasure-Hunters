using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunters
{
    public class CaptainSpecialAttackTwo : MonoBehaviour
    {
        private Animator playerAnimator;
        [SerializeField] private GameObject sword;

        void Start()
        {
            playerAnimator = GetComponent<Animator>();
            playerAnimator.SetBool("Has Sword", true);
        }

        private void ThrowSword()
        {
            playerAnimator.SetBool("Has Sword",false);
            sword.SetActive(true);
        }

        public void RecoverSword()
        {
            playerAnimator.SetBool("Has Sword", true);
            sword.SetActive(false);
        }
    }
}
