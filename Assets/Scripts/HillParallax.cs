using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class HillParallax : MonoBehaviour
    {
        public float protrusionFactor = 1.5f;
        private float startYPosition;

        private void Start()
        {
            startYPosition = transform.position.y;
        }

        public void UpdateParallax(float playerYPosition)
        {
            float yOffset = startYPosition - playerYPosition;
            //Debug.Log("player y pos: " + playerYPosition);
            //Debug.Log("hill parallax y pos: " + transform.position.y);
            //Debug.Log("y offset: " + yOffset);

            float protrusionOffset = protrusionFactor * Mathf.Pow(yOffset, 1f);

            transform.position = new Vector3(transform.position.x, playerYPosition + protrusionOffset, transform.position.z);
        }

        public void ScrollHill(float forwardDistance, float scrollSpeed)
        {
            transform.position -= new Vector3(0, forwardDistance * scrollSpeed * Time.deltaTime, 0);
        }
    }
}
