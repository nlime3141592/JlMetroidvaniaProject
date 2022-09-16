/*
using UnityEngine;
using UnityEngine.SceneManagement;

using JlMetroidvaniaProject.Controllers;

namespace JlMetroidvaniaProject.Maps
{
    public class MapTeleporter : JlBehaviour
    {
        public int portalID;

        private PlayerController playerCtrl;

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if(playerCtrl == null)
            {
                PlayerController.GetPlayerControllerByTrigger2D(collider, out playerCtrl);
                
                // TODO: 다음 장면 로딩을 위한 로직을 여기서 수행한다.
                int currentMap = gameObject.scene.buildIndex;
                int nextMap = GameManager.s_m_mapNetwork.GetNextMap(currentMap, portalID);
                float x, y;
                GameManager.s_m_mapNetwork.GetDestination(nextMap, portalID, out x, out y);
                GameManager.s_gameManager.ChangeMap(currentMap, nextMap, new Vector2(x, y));
                Debug.Log("이동되었습니다.");
            }
        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            PlayerController ctrl;
            bool canGet = PlayerController.GetPlayerControllerByTrigger2D(collider, out ctrl);

            if(canGet && ctrl == playerCtrl)
            {
                playerCtrl = null;
            }
        }
    }
}
*/