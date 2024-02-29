using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Plattko
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCam;
        private CinemachineFramingTransposer framingTransposer;
        private Transform followTarget;
        [SerializeField] private Transform dummyFollowTarget;

        private Vector3 startOffset;
        private Vector3 panOutOffset = new Vector3(0f, 5f, 0f);
        private Vector3 velocity = Vector3.zero;
        private float smoothTime = 2f;

        private bool isPannedOut = false;
        public bool isActive = true;

        private void Start()
        {
            virtualCam = GetComponent<CinemachineVirtualCamera>();
            framingTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            followTarget = virtualCam.Follow;

            startOffset = framingTransposer.m_TrackedObjectOffset;
        }

        public void PanOut()
        {
            if (!isPannedOut)
            {
                dummyFollowTarget.position = virtualCam.transform.position;
                virtualCam.Follow = dummyFollowTarget;
                isPannedOut = true;
            }
            
            framingTransposer.m_TrackedObjectOffset = Vector3.SmoothDamp(framingTransposer.m_TrackedObjectOffset, panOutOffset, ref velocity, smoothTime);
        }

        public void PanIn()
        {
            if (isPannedOut)
            {
                float yTrackingOffset = virtualCam.transform.position.y - followTarget.position.y;
                framingTransposer.m_TrackedObjectOffset = new Vector3(framingTransposer.m_TrackedObjectOffset.x, yTrackingOffset, framingTransposer.m_TrackedObjectOffset.z);
                virtualCam.Follow = followTarget;
                isPannedOut = false;
            }

            framingTransposer.m_TrackedObjectOffset = Vector3.SmoothDamp(framingTransposer.m_TrackedObjectOffset, startOffset, ref velocity, smoothTime);
        }

        public void ReactivateCamera()
        {
            isActive = true;
        }
    }
}
