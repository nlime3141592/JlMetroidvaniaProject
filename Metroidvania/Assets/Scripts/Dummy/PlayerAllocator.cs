using UnityEngine;
using Cinemachine;

namespace JlMetroidvaniaProject.Dummy
{
    public class PlayerAllocator : MonoBehaviour
    {
        private CinemachineVirtualCamera m_vCam;

        void Start()
        {
            m_vCam = GetComponent<CinemachineVirtualCamera>();
        }

        void Update()
        {
            m_vCam.m_Follow = GameManager.s_playerTransform;
        }
    }
}