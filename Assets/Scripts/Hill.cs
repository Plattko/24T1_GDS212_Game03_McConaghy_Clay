using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class Hill : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        public Vector3 currentMinScale;

        public bool isLastHill = false;

        private void Start()
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !isLastHill)
            {
                Debug.Log("Collision detected.");
                PlayerController playerController = collision.GetComponent<PlayerController>();

                if (playerController.isOnNearSide)
                {
                    playerController.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
                    playerController.isOnNearSide = false;
                }
                else if (!playerController.isOnNearSide)
                {
                    playerController.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
                    playerController.isOnNearSide = true;
                }
            }
        }
    }
}
