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
        private float nextHillDistance = 22f;

        [Header("Hill Scale Variables")]
        private Vector3 minScale = new Vector3(8f, 8f, 1f);
        private Vector3 maxScale = new Vector3(14f, 14f, 1f);
        private Vector3 previousHillScale = new Vector3(32f, 32f, 1f);

        private float scaleInForwardDistance = 1f;
        private float noScaleForwardDistance = 15f;

        private int currentHill = 0;
        private bool isInHillTransition = false;

        [Header("Camera Follow Variables")]
        private float reactivateCameraForwardDistance = 2.8f;

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
            else if (cameraController.isActive)
            {
                cameraController.PanIn();
                Debug.Log("Pan in condition met. isActive: " + cameraController.isActive);
            }

            // Update hill parallaxes
            if (!isInHillTransition)
            {
                for (int i = currentHill + 1; i < hillParallaxes.Length; i++)
                {
                    HillParallax hillParallax = hillParallaxes[i];
                    hillParallax.UpdateParallax(playerController.forwardDistance);
                }

                if (currentHill > 0)
                {
                    hillParallaxes[currentHill - 1].UpdateParallax(playerController.forwardDistance);
                }
            }

            // Transition player to next hill
            if (playerController.forwardDistance > nextHillDistance)
            {
                StartCoroutine(HillForwardTransition());
            }

            //Reactivate camera if player reaches reactivation distance
            if (playerController.forwardDistance >= reactivateCameraForwardDistance && !cameraController.isActive)
            {
                cameraController.ReactivateCamera();
            }
        }

        private IEnumerator HillForwardTransition()
        {
            Debug.Log("Began hill foward transition.");
            // Set is in hill transition to true
            isInHillTransition = true;
            // Set camera to not actively follow player
            cameraController.isActive = false;
            // Reset the player's forward distance
            playerController.forwardDistance = 0f;
            // Disable player input
            playerController.isInHillTransition = true;

            // Disable current hill's box collider
            hills[currentHill].GetComponent<BoxCollider2D>().enabled = false;
            // Enable the next hill's box collider
            hills[currentHill + 1].GetComponent<BoxCollider2D>().enabled = true;

            // Update the current hill
            currentHill++;

            // Update all hills' current min sizes
            UpdateHillScales();
            // Update all hills' start positions
            yield return StartCoroutine(UpdateDistantHillPositions());
            // Update all distant hills' parallaxes
            int index = 0;
            for (int i = currentHill + 1; i < hillParallaxes.Length; i++)
            {
                HillParallax hillParallax = hillParallaxes[i];
                hillParallax.SetHillParallax(index);
                index++;
            }
            // Update previous hill's parallax
            hillParallaxes[currentHill - 1].SetPreviousHillParallax();

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
            // Set is in hill transition to false
            isInHillTransition = false;

            Debug.Log("Hill forward transition complete.");

            yield return null;
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
                Hill previousHillData = hills[i - 1].transform.GetComponent<Hill>();

                if (previousHillData != null)
                {
                    hill.localScale = Vector3.Lerp(hillData.currentMinScale, previousHillData.currentMinScale, t);
                }
            }
        }

        public IEnumerator UpdateDistantHillPositions()
        {
            Vector3 currentHillPosition = hills[currentHill].transform.position;
            Debug.Log("Current hill position: " + currentHillPosition);

            int index = 0;
            float smoothTime = 0.5f;

            for (int i = currentHill + 1; i < hills.Length; i++)
            {
                Transform hill = hills[i].transform;
                Vector3 targetPosition = Vector3.zero;

                if (index == 0)
                {
                    targetPosition = new Vector3(currentHillPosition.x, currentHillPosition.y + 2.1f, currentHillPosition.z);
                    Debug.Log(hills[i] + " position: " + hill.position);
                }
                else if (index > 0)
                {
                    targetPosition = new Vector3(currentHillPosition.x, currentHillPosition.y + 4.1f, currentHillPosition.z);
                    Debug.Log(hills[i] + " position: " + hill.position);
                }

                float elapsedTime = 0f;
                Vector3 velocity = Vector3.zero;

                if (index <= 1)
                {
                    while (elapsedTime < smoothTime)
                    {
                        hill.position = Vector3.SmoothDamp(hill.position, targetPosition, ref velocity, smoothTime);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                }
                else
                {
                    hill.position = targetPosition;
                }
                Debug.Log("Finished hill " + i + "position transition.");

                index++;
            }
        }
    }
}
