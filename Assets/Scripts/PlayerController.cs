using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TreasureHunters
{
    public class PlayerController : MonoBehaviour
    {
        private Animator playerAnimator;
        private PlayerLifeController playerLifeController;

        private Rigidbody2D rB2D;
        private Vector2 movementInput;
        private float jumpForce;

        private float coyoteTime = 0.2f;
        private float coyoteTimeCounter;

        private float jumpBufferTime = 0.2f;
        private float jumpBufferCounter;

        private bool isAttacking;
        [SerializeField] private float attackDelay;
        [SerializeField] private Transform firstAttackZone;
        [SerializeField] private Transform secondAttackZone;
        [SerializeField] private float attackZoneRadius;
        [SerializeField] private LayerMask isAttackable;
        [SerializeField] private int damage;
        private float attackTimeCounter;

        [SerializeField] private float speed;
        [SerializeField] private Transform checkGroundZone;
        [SerializeField] private float groundCheckerRadius;
        [SerializeField] private LayerMask isGround;

        private void Start()
        {
            rB2D = GetComponent<Rigidbody2D>();
            playerAnimator = GetComponent<Animator>();
            playerLifeController = GetComponent<PlayerLifeController>();
            jumpForce = 15f;
        }

        // Update is called once per frame
        private void Update()
        {
            Walk();

            Jump();

            Attack();

            playerAnimator.SetFloat("VelX", Mathf.Abs(rB2D.velocity.x));
            playerAnimator.SetFloat("VelY", rB2D.velocity.y);
            playerAnimator.SetBool("IsGrounded", IsGrounded());
        }

        private void Walk()
        {
            if (isAttacking || playerLifeController.isHited) return;
            movementInput = new Vector2(Input.GetAxis("Horizontal") * speed, rB2D.velocity.y);

            if (movementInput.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (movementInput.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

        }

        private void Jump()
        {
            if (isAttacking || playerLifeController.isHited) return;

            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
            {
                rB2D.velocity = new Vector2(rB2D.velocity.x, jumpForce);
                jumpBufferCounter = 0f;
            }
            if (Input.GetKeyUp(KeyCode.Space) && rB2D.velocity.y > 0f)
            {
                rB2D.velocity = new Vector2(rB2D.velocity.x, rB2D.velocity.y * 0.5f);
                coyoteTimeCounter = 0f;
            }
        }

        private void Attack()
        {
            if (attackTimeCounter > 0f)
            {
                attackTimeCounter -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.U) && IsGrounded() && attackTimeCounter <= 0f)
            {
                rB2D.velocity = Vector2.zero;
                movementInput = Vector2.zero;
                playerAnimator.SetTrigger("Attack");
                attackTimeCounter = attackDelay;
            }
        }

        private void PerformAttack()
        {
            if (secondAttackZone != null)
            {
                Collider2D[] charactersReachedFirstZone = Physics2D.OverlapCircleAll(firstAttackZone.position, attackZoneRadius, isAttackable);
                foreach (Collider2D character in charactersReachedFirstZone)
                {
                    character.GetComponent<PlayerLifeController>().TakeDamage(damage);
                }
                Collider2D[] charactersReachedSecondZone = Physics2D.OverlapCircleAll(secondAttackZone.position, attackZoneRadius, isAttackable);
                foreach (Collider2D character in charactersReachedSecondZone)
                {
                    character.GetComponent<PlayerLifeController>().TakeDamage(damage);
                }
            }
            else
            {
                Collider2D[] charactersReached = Physics2D.OverlapCircleAll(firstAttackZone.position, attackZoneRadius, isAttackable);
                foreach (Collider2D character in charactersReached)
                {
                    character.GetComponent<PlayerLifeController>().TakeDamage(damage);
                }
            }

        }

        private void AttackStarts()
        {
            isAttacking = true;
        }

        private void AttackWithMovementStarts()
        {
            rB2D.velocity = new Vector2(transform.localScale.x * speed * 1.5f, rB2D.velocity.y);
        }

        private void AttackWithMovementEnds()
        {
            rB2D.velocity = Vector2.zero;
        }

        private void AttackEnds()
        {
            isAttacking = false;
        }

        private void FixedUpdate()
        {
            if (isAttacking || playerLifeController.isHited) return;
            rB2D.velocity = movementInput;
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(checkGroundZone.position, groundCheckerRadius, isGround);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(checkGroundZone.position, groundCheckerRadius);

            Gizmos.color = Color.cyan;
            if (secondAttackZone != null)
            {
                Gizmos.DrawWireSphere(firstAttackZone.position, attackZoneRadius);
                Gizmos.DrawWireSphere(secondAttackZone.position, attackZoneRadius);
            }
            else
            {
                Gizmos.DrawWireSphere(firstAttackZone.position, attackZoneRadius);
            }
        }
    }
}
