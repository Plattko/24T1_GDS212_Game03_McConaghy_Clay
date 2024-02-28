using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Plattko
{
    public class HillManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] hills;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CinemachineVirtualCamera virtualCam;

        private Vector3 newHillPlayerScale = new Vector3(0.05f, 0.05f, 1f);
        private float transitionTime = 2f;
        private float nextHillDistance = 14f;

        [Header("Hill Scale Variables")]
        private Vector3 minScale = new Vector3(8f, 8f, 1f);
        private Vector3 maxScale = new Vector3(20f, 20f, 1f);

        private float scaleInForwardDistance = 1f;
        private float noScaleForwardDistance = 8.5f;
        //private float scaleOutForwardDistance = 11f;

        //private Vector3 scaleVelocity = Vector3.zero;

        //[Header("Camera Zoom Variables")]
        //private float minZoom = 25f;
        //private float maxZoom = 10f;
        //private float camVelocity = 0f;

        private int currentHill = 0;

        void Start()
        {
            //for (int i = 1; i < hills.Length; i++)
            //{
            //    hills[i].transform.localScale = new Vector3(hills[i - 1].transform.localScale.x / 2, hills[i - 1].transform.localScale.y / 2, 1f);
            //    Debug.Log("Hill local scale: " + hills[i].transform.localScale);
            //}
        }

        void Update()
        {
            if (playerController.forwardDistance >= scaleInForwardDistance && playerController.forwardDistance <= noScaleForwardDistance)
            {
                float t = Mathf.InverseLerp(scaleInForwardDistance, noScaleForwardDistance, playerController.forwardDistance);
                hills[currentHill].transform.localScale = Vector3.Lerp(minScale, maxScale, t);
            }

            //if (playerController.forwardDistance > scaleOutForwardDistance)
            //{
            //    virtualCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCam.m_Lens.OrthographicSize, minZoom, ref camVelocity, 1f);
            //    //hills[currentHill].transform.localScale = Vector3.SmoothDamp(hills[currentHill].transform.localScale, minScale, ref scaleVelocity, 1f);
            //}

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

            // Set new player sprite sorting order
            int newSortingOrder = hills[currentHill].transform.GetComponent<SpriteRenderer>().sortingOrder + 1;
            playerController.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = newSortingOrder;
            // Set player to near side of hill
            playerController.isOnNearSide = true;
            // Set new player parent
            playerController.transform.parent = hills[currentHill].transform;
            // Set new player scale
            playerController.transform.localScale = newHillPlayerScale;
            // Set new player position
            playerController.transform.localPosition = new Vector2(playerController.transform.localPosition.x, -0.4f);
            // Re-enable player input
            playerController.isInHillTransition = false;

            Debug.Log("Hill forward transition complete.");
        }
    }
}
