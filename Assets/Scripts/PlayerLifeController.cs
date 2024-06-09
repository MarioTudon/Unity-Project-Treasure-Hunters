using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunters
{
    public class PlayerLifeController : MonoBehaviour
    {
        [SerializeField] private int life;
        [SerializeField] private Text lifeText;
        private Animator playerAnimator;
        private Rigidbody2D rB2D;
        [HideInInspector] public bool isHited;

        private void Start()
        {
            playerAnimator = GetComponent<Animator>();
            rB2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            lifeText.text = "Life: " + life;
            playerAnimator.SetInteger("Life", life);
        }

        public void TakeDamage(int damage)
        {
            if (life <= 0) return;
            life -= damage;
            rB2D.velocity = Vector2.zero;
            playerAnimator.SetTrigger("Hit");
        }

        private void HitStarts()
        {
            isHited = true;
        }

        private void HitEnds()
        {
            isHited = false;
        }
    }
}
