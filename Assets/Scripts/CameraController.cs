using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Plattko
{
    public class CameraController : MonoBehaviour
    {
        private enum CameraState
        {
            FarPan,
            StillPan,
            HillTopPan
        }

        private CameraState cameraState;
        
        [SerializeField] private Transform hill;
        [SerializeField] private Transform player;
        private CinemachineVirtualCamera virtualCam;
        private PlayerController playerController;

        [Header("Ortho bounds")]
        [SerializeField] private float maxOrthoSize = 16f;
        [SerializeField] private float minOrthoSize = 8f;
        private float orthoSize;

        [Header("Camera behaviour points")]
        [SerializeField] private float stopPanInDistance = 0.25f;
        [SerializeField] private float panOutDistance = 0.45f;

        private float stillPanFowardDistance = 0.08f;
        private float panOutForwardDistance = 0.145f;
        
        // Smooth damping
        private float velocity = 0f;
        private float smoothTime = 0.1f;
        
        private void Start()
        {
            cameraState = CameraState.FarPan;
            virtualCam = GetComponent<CinemachineVirtualCamera>();
            playerController = player.GetComponent<PlayerController>();
            orthoSize = virtualCam.m_Lens.OrthographicSize;
        }

        private void Update()
        {
            Vector3 playerPos = hill.InverseTransformPoint(player.position);

            //// Make camera ortho size relative to player position on hill
            //if (playerPos.y < stopPanInDistance)
            //{
            //    float distanceFromTop = playerPos.y - 0.5f;
            //    //Debug.Log("Distance from top: " + distanceFromTop);
            //    Debug.Log("Player Y pos: " + playerPos.y);
            //    orthoSize = Mathf.Abs(distanceFromTop) * 20f;
            //    orthoSize = Mathf.Clamp(orthoSize, minOrthoSize, maxOrthoSize);
            //    virtualCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCam.m_Lens.OrthographicSize, orthoSize, ref velocity, smoothTime);
            //}
            //else if (playerPos.y > panOutDistance)
            //{
            //    virtualCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCam.m_Lens.OrthographicSize, 20f, ref velocity, 1f);
            //}

            switch (cameraState)
            {
                case CameraState.FarPan:

                    float distanceFromTop = playerPos.y - 0.5f;
                    //Debug.Log("Distance from top: " + distanceFromTop);
                    //Debug.Log("Player Y pos: " + playerPos.y);
                    orthoSize = Mathf.Abs(distanceFromTop) * 20f;
                    orthoSize = Mathf.Clamp(orthoSize, minOrthoSize, maxOrthoSize);
                    virtualCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCam.m_Lens.OrthographicSize, orthoSize, ref velocity, smoothTime);

                    if (playerController.forwardDistance >= stillPanFowardDistance)
                    {
                        Debug.Log("Changed to StillPan state.");
                        cameraState = CameraState.StillPan;
                    }
                    break;

                case CameraState.StillPan:
                    
                    if (playerController.forwardDistance < stillPanFowardDistance)
                    {
                        Debug.Log("Changed to FarPan state.");
                        cameraState = CameraState.FarPan;
                    }

                    if (playerController.forwardDistance >= panOutForwardDistance)
                    {
                        Debug.Log("Changed to PanOut state.");
                        // Pan out
                        virtualCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCam.m_Lens.OrthographicSize, 20f, ref velocity, 1f);
                        cameraState = CameraState.HillTopPan;
                    }
                    break;

                case CameraState.HillTopPan:
                    if (playerController.forwardDistance < panOutForwardDistance)
                    {
                        Debug.Log("Changed to StillPan state.");
                        // Pan back in
                        virtualCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(virtualCam.m_Lens.OrthographicSize, 8f, ref velocity, 1f);
                        cameraState = CameraState.StillPan;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
