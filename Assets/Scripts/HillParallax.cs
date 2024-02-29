using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class HillParallax : MonoBehaviour
    {
        //public float protrusionFactor = 1.5f;
        //private float startYPosition;

        //private void Start()
        //{
        //    startYPosition = transform.position.y;
        //}

        //public void UpdateParallax(float playerYPosition)
        //{
        //    float yOffset = startYPosition - playerYPosition;
        //    //Debug.Log("player y pos: " + playerYPosition);
        //    //Debug.Log("hill parallax y pos: " + transform.position.y);
        //    //Debug.Log("y offset: " + yOffset);

        //    float protrusionOffset = protrusionFactor * Mathf.Pow(yOffset, 1f);

        //    transform.position = new Vector3(transform.position.x, playerYPosition + protrusionOffset, transform.position.z);
        //}

        //public void ScrollHill(float forwardDistance, float scrollSpeed)
        //{
        //    transform.position -= new Vector3(0, forwardDistance * scrollSpeed * Time.deltaTime, 0);
        //}

        [SerializeField] private int hillIndex = 0;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Vector2 targetPosition;

        private float startLerpDistance = 0f;
        private float stopLerpDistance = 15f;

        // Set hill parallax
        public void SetHillParallax(int index)
        {
            startPosition = transform.position;
            
            hillIndex = index;

            if (index == 0)
            {
                targetPosition = new Vector2(startPosition.x, startPosition.y + 13.8f);
            }
            else if (index == 1)
            {
                targetPosition = new Vector2(startPosition.x, startPosition.y + 15f);
            }
            else if (index == 2)
            {
                targetPosition = new Vector2(startPosition.x, startPosition.y + 10f);
            }
            else
            {
                Debug.Log("Index " + hillIndex + "not included.");
            }
        }

        // Lerp hill based on hill index
        public void UpdateParallax(float forwardDistance)
        {
            float t = Mathf.InverseLerp(startLerpDistance, stopLerpDistance, forwardDistance);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
    }
}
