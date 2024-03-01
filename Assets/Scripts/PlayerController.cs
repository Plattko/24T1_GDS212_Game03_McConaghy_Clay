using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Plattko
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rb;
        [HideInInspector] public SpriteRenderer spriteRenderer;
        
        private Vector2 moveInput;
        
        [SerializeField] private float walkSpeed = 5f;
        [HideInInspector] public bool isOnNearSide = true;

        // Hill traversal variables
        public float forwardDistance = 0f;
        public bool isInHillTransition = false;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (!isInHillTransition)
            {
                if (isOnNearSide)
                {
                    rb.velocity = moveInput * walkSpeed * Time.deltaTime * 50f;
                }
                else
                {
                    rb.velocity = new Vector2(moveInput.x, -moveInput.y) * walkSpeed * Time.deltaTime * 50f;
                }

                forwardDistance += moveInput.y * walkSpeed * Time.deltaTime;
                Debug.Log("Forward distance: " + forwardDistance);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!isInHillTransition)
            {
                moveInput = context.ReadValue<Vector2>();
            }
            else
            {
                moveInput = Vector2.zero;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!isInHillTransition)
            {
                Debug.Log("Interact button pressed.");
            }
        }
    }
}
