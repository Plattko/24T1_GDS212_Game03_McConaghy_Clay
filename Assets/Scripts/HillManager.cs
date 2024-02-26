using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plattko
{
    public class HillManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] hills;
        [SerializeField] private PlayerController playerController;

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
            
        }
    }
}
