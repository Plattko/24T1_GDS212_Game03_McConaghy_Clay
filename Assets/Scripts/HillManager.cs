using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class HillManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] hills;
        [SerializeField] private HillParallax[] hillParallaxes;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CameraController cameraController;

        private Vector3 newHillPlayerScale = new Vector3(0.05f, 0.05f, 1f);
        private float transitionTime = 2f;
        private float nextHillDistance = 22f;

        [Header("Hill Scale Variables")]
        private Vector3 minScale = new Vector3(8f, 8f, 1f);
        private Vector3 maxScale = new Vector3(14f, 14f, 1f);
        private Vector3 previousHillScale = new Vector3(32f, 32f, 1f);

        private float scaleInForwardDistance = 1f;
        private float noScaleForwardDistance = 15f;

        private int currentHill = 0;

        void Start()
        {
            int highestSortOrder = hills.Length * 5;
            
            // Set hill sorting orders
            for (int i = 0; i < hills.Length; i++)
            {
                Transform hill = hills[i].transform;
                hill.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(highestSortOrder - (i * 5));
                Debug.Log("Hill " + i + " sorting order: " + hill.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder);
            }

            // Set player sorting order
            playerController.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = highestSortOrder + 1;

            // Set hill scales
            SetHillScales();

            // Set hill parallaxes
            int index = 0;
            for (int i = currentHill + 1; i < hillParallaxes.Length; i++)
            {
                HillParallax hillParallax = hillParallaxes[i];
                hillParallax.SetHillParallax(index);
                Debug.Log("Hill parallax " + hillParallaxes[i] + "hill index: " + index);
                index++;
            }
        }

        void Update()
        {
            if (playerController.forwardDistance >= scaleInForwardDistance && playerController.forwardDistance <= noScaleForwardDistance)
            {
                float t = Mathf.InverseLerp(scaleInForwardDistance, noScaleForwardDistance, playerController.forwardDistance);
                // Scale the current hill
                hills[currentHill].transform.localScale = Vector3.Lerp(minScale, maxScale, t);
                // Scale the distant hills
                ScaleDistantHills(t);
                // Scale the previous hill
                if (currentHill > 0)
                {
                    hills[currentHill - 1].transform.localScale = Vector3.Lerp(maxScale, previousHillScale, t);
                }
            }

            // Pan out camera when player is at top of hill
            if (playerController.forwardDistance >= noScaleForwardDistance)
            {
                cameraController.PanOut();
            }
            else
            {
                cameraController.PanIn();
            }

            // Update hill parallaxes
            for (int i = currentHill + 1; i < hillParallaxes.Length; i++)
            {
                HillParallax hillParallax = hillParallaxes[i];
                hillParallax.UpdateParallax(playerController.forwardDistance);
            }

            // Transition player to next hill
            if (playerController.forwardDistance > nextHillDistance)
            {
                StartCoroutine(HillForwardTransition());
            }
        }

        private IEnumerator HillForwardTransition()
        {
            Debug.Log("Began hill foward transition.");
            playerController.forwardDistance = 0f;
            // Disable player input
            playerController.isInHillTransition = true;

            yield return new WaitForSeconds(transitionTime);

            hills[currentHill].GetComponent<BoxCollider2D>().enabled = false;

            currentHill++;

            // Update all hills' current min sizes
            UpdateHillScales();
            // Set new player sprite sorting order
            int newSortingOrder = hills[currentHill].transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder + 1;
            playerController.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = newSortingOrder;
            Debug.Log("Player new sorting order: " + newSortingOrder);
            // Set player to near side of hill
            playerController.isOnNearSide = true;
            // Set new player parent
            playerController.transform.parent = hills[currentHill].transform;
            // Set new player scale
            playerController.transform.localScale = newHillPlayerScale;
            // Set new player position
            playerController.transform.localPosition = new Vector2(playerController.transform.localPosition.x, -1.337f);
            // Re-enable player input
            playerController.isInHillTransition = false;

            Debug.Log("Hill forward transition complete.");
        }

        public void SetHillScales()
        {
            for (int i = 0; i < hills.Length; i++)
            {
                Transform hill = hills[i].transform;

                float t = (float)i / (hills.Length - 1);
                float scaleFactor = Mathf.Lerp(8f, 2f, t);

                hill.localScale = new Vector3(scaleFactor, scaleFactor, 1f);

                hill.GetComponent<Hill>().currentMinScale = hill.localScale;
            }
        }

        public void UpdateHillScales()
        {
            for (int i = 0; i < hills.Length; i++)
            {
                Transform hill = hills[i].transform;
                hill.GetComponent<Hill>().currentMinScale = hill.localScale;
            }
        }

        public void ScaleDistantHills(float t)
        {
            for (int i = currentHill + 1; i < hills.Length; i++)
            {
                Transform hill = hills[i].transform;
                Hill hillData = hill.GetComponent<Hill>();
                Hill previousHilLData = hills[i - 1].transform.GetComponent<Hill>();

                if (previousHilLData != null)
                {
                    hill.localScale = Vector3.Lerp(hillData.currentMinScale, previousHilLData.currentMinScale, t);
                }
            }
        }
    }
}
