using UnityEngine;

using JlMetroidvaniaProject.Controllers;

namespace JlMetroidvaniaProject.Maps
{
    public class Teleporter : JlBehaviour
    {
        [HideInInspector]
        public int portalID;

        private PlayerController m_playerController;

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if(m_playerController == null)
            {
                PlayerController.GetPlayerControllerByTrigger2D(collider, out m_playerController);

                int currentMap;
                int nextMap;
                float destiX;
                float destiY;

                currentMap = gameObject.scene.buildIndex;
                nextMap = GameManager.s_mapNetwork.GetNextMap(currentMap, portalID);
                GameManager.s_mapNetwork.GetDestination(nextMap, portalID, out destiX, out destiY);

                // NOTE: 다음 맵을 로딩합니다.
                GameManager.s_gameManager.ChangeMap(currentMap, nextMap, new Vector2(destiX, destiY));
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