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

        private Vector3 startOffset;
        private Vector3 panOutOffset = new Vector3(0f, 5f, 0f);
        private Vector3 velocity = Vector3.zero;
        private float smoothTime = 2f;

        private void Start()
        {
            virtualCam = GetComponent<CinemachineVirtualCamera>();
            framingTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            startOffset = framingTransposer.m_TrackedObjectOffset;
        }

        public void PanOut()
        {
            framingTransposer.m_TrackedObjectOffset = Vector3.SmoothDamp(framingTransposer.m_TrackedObjectOffset, panOutOffset, ref velocity, smoothTime);
        }

        public void PanIn()
        {
            framingTransposer.m_TrackedObjectOffset = Vector3.SmoothDamp(framingTransposer.m_TrackedObjectOffset, startOffset, ref velocity, smoothTime);
        }
    }
}
