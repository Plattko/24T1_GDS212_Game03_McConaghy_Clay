using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class HillManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] hills;
        [SerializeField] private PlayerController playerController;

        private Vector3 newHillPlayerScale = new Vector3(0.1f, 0.1f, 1f);
        private float transitionTime = 2f;
        private float nextHillDistance = 9.3f;

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
            if (playerController.forwardDistance > nextHillDistance)
            {
                StartCoroutine(HillForwardTransition());
            }
        }

        private IEnumerator HillForwardTransition()
        {
            Debug.Log("Began hill foward transition.");
            playerController.forwardDistance = 0f;

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
            playerController.transform.localPosition = new Vector2(playerController.transform.localPosition.x, -0.45f);

            Debug.Log("Hill forward transition complete.");
        }
    }
}
