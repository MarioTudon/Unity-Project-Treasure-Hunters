using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunters
{
    enum Layers{
        Attackable = 6
    }
    public class Sword : MonoBehaviour
    {
        private Rigidbody2D rB2D;
        private bool isEmbedded;
        private Animator swordAnimator;
        private Transform parentTransform;
        private Vector2 originalPosition;
        private float xDirection;
        [SerializeField] private float speed;

        void Start()
        {
            rB2D = GetComponent<Rigidbody2D>();
            swordAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            isEmbedded = false;
            parentTransform = transform.parent;
            xDirection = transform.parent.localScale.x;
            transform.parent = null;
        }

        void Update()
        {
            if (isEmbedded) return;
            rB2D.velocity = new Vector2(xDirection, 0f) * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Nestable")
            {
                isEmbedded = true;
                rB2D.velocity = Vector2.zero;
                swordAnimator.SetTrigger("Is Embedded");
            }
            else if(collision.transform == parentTransform && isEmbedded)
            {
                transform.parent = parentTransform;
                transform.localPosition = new Vector2(1, 0);
                collision.gameObject.GetComponent<CaptainSpecialAttackTwo>().RecoverSword();
            }

        }
    }
}
