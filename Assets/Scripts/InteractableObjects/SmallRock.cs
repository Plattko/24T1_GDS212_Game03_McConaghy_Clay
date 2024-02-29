using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class SmallRock : InteractRadius
    {
        private Transform hillTransform;
        private Transform playerTransform;

        private bool isHeldByPlayer = false;

        private void Start()
        {
            hillTransform = transform.parent;
        }

        private void Update()
        {
            if (isHeldByPlayer)
            {
                transform.position = playerTransform.TransformPoint(0.3f, 0f, 1f);
            }
        }

        public override void Interact()
        {
            if (!isHeldByPlayer)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                isHeldByPlayer = true;
            }
            else
            {
                isHeldByPlayer = false;
                // Set position to on the floor next to the player
                transform.position = playerTransform.TransformPoint(0f, -0.5f, 1f);
                // Change the rock's parent object back to the hill
                transform.parent = hillTransform;
            }
        }
    }
}
