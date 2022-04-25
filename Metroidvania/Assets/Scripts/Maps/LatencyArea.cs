using UnityEngine;

using JlMetroidvaniaProject.Controllers;

namespace JlMetroidvaniaProject.Maps
{
    // TODO: OnTriggerEnter2D로 PlayerController.OnLatencyAreaEnter()를 호출해서 조종 제어를 하게 되면,
    // 1회성 호출만으로 플레이어를 이동시킬 수 있다.
    // PlayerController에서 moveDirection에 기반해 이동 로직을 계속 수행하고 있기 때문.
    public class LatencyArea : JlBehaviour
    {
        public float direction;

        private PlayerController m_playerController;

        protected override void Update()
        {
            if(m_playerController != null)
            {
                m_playerController.OnLatencyArea(direction);
            }
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if(m_playerController == null)
            {
                PlayerController.GetPlayerControllerByTrigger2D(collider, out m_playerController);
            }
        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            PlayerController playerController;
            bool canGet;

            canGet = PlayerController.GetPlayerControllerByTrigger2D(collider, out playerController);

            if(canGet && playerController == m_playerController)
            {
                m_playerController = null;
            }
        }
    }
}