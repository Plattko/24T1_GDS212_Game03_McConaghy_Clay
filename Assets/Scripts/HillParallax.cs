using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class HillParallax : MonoBehaviour
    {
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
            else if (index > 0)
            {
                targetPosition = new Vector2(startPosition.x, startPosition.y + 15f);
            }

            Debug.Log("Hill " + (index + 1) + " start position: " + startPosition);
            Debug.Log("Hill " + (index + 1) + " target position: " + targetPosition);
        }

        // Lerp hill based on hill index
        public void UpdateParallax(float forwardDistance)
        {
            float t = Mathf.InverseLerp(startLerpDistance, stopLerpDistance, forwardDistance);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
    }
}
