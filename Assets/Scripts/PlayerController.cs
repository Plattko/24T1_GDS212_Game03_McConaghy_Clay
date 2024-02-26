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

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (isOnNearSide)
            {
                rb.velocity = moveInput * walkSpeed * Time.deltaTime * 50f;
            }
            else
            {
                rb.velocity = new Vector2(moveInput.x, -moveInput.y) * walkSpeed * Time.deltaTime * 50f;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Debug.Log("Interact button pressed.");
        }
    }
}
