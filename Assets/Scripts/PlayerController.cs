using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Plattko
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject moveJoystick;
        public GameObject interactButton;

        private Rigidbody2D rb;
        [HideInInspector] public SpriteRenderer spriteRenderer;
        
        private Vector2 moveInput;
        
        [SerializeField] private float walkSpeed = 5f;
        [HideInInspector] public bool isOnNearSide = true;

        // Hill traversal variables
        public float forwardDistance = 0f;
        public bool isInHillTransition = false;
        public bool isInteractButtonVisible = false;

        void Awake()
        {
            Physics2D.callbacksOnDisable = false;
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

                forwardDistance += moveInput.y * Time.deltaTime;
                //Debug.Log("Forward distance: " + forwardDistance);
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
            if (context.started && !isInHillTransition && isInteractButtonVisible)
            {
                Debug.Log("Interact button pressed.");

                // Interact with the closest interactable object
                InteractRadius[] interactableObjects = GameObject.FindObjectsByType<InteractRadius>(FindObjectsSortMode.None);
                GetClosestInteractableObject(interactableObjects).Interact();
            }
        }

        private InteractRadius GetClosestInteractableObject(InteractRadius[] interactableObjects)
        {
            InteractRadius closestObject = null;
            float minDistance = Mathf.Infinity;

            foreach (InteractRadius interactableObject in interactableObjects)
            {
                float distance = (interactableObject.transform.position - transform.position).sqrMagnitude;

                if (distance < minDistance)
                {
                    closestObject = interactableObject;
                    minDistance = distance;
                }
            }
            return closestObject;
        }
    }
}
