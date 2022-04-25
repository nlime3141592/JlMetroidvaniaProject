using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using JlMetroidvaniaProject.Utility;
using JlMetroidvaniaProject.Datas;
using JlMetroidvaniaProject.Physics;
using JlMetroidvaniaProject.Maps;

namespace JlMetroidvaniaProject
{
    public sealed class GameManager : JlBehaviour
    {
        public static GameManager s_gameManager
        {
            get
            {
                if(s_m_gameManager == null)
                    throw new NullReferenceException("Game Manager is NULL.");

                return s_m_gameManager;
            }
        }

        public static InputSystem s_inputSystem
        {
            get
            {
                if(s_m_inputSystem == null)
                    throw new NullReferenceException("Input System is NULL.");

                return s_m_inputSystem;
            }
        }

        public static MapNetwork s_mapNetwork
        {
            get
            {
                if(s_m_mapNetwork == null)
                    throw new NullReferenceException("Map Network is NULL.");

                return s_m_mapNetwork;
            }
        }

        public static Transform s_playerTransform
        {
            get
            {
                if(s_m_playerTransform == null)
                    throw new NullReferenceException("Player is NULL.");

                return s_m_playerTransform;
            }
        }

        private static InputState s_m_inputState;
        private static InputSystem s_m_inputSystem;
        private static MapNetwork s_m_mapNetwork;
        private static Transform s_m_playerTransform;
        private static GameManager s_m_gameManager;

        public Transform playerTransform;

        protected override void Awake()
        {
            base.Awake();

            s_m_inputState = new InputState();
            s_m_inputSystem = new InputSystem(s_m_inputState);
            s_m_mapNetwork = new MapNetwork(1);

            if(s_m_playerTransform == null)
                s_m_playerTransform = playerTransform;

            s_m_mapNetwork.m_mapNetwork[0, 0] = 1;
            s_m_mapNetwork.m_mapNetwork[1, 0] = 2;

            s_m_mapNetwork.m_destiTable[0, 0] = 66.0f;
            s_m_mapNetwork.m_destiTable[1, 0] = 13.0f;
            s_m_mapNetwork.m_destiTable[2, 0] = 24.0f;
            s_m_mapNetwork.m_destiTable[3, 0] = 4.5f;

            if(s_m_gameManager == null)
                s_m_gameManager = this;
            else
                DestroyImmediate(this.gameObject);
        }

        protected override void Update()
        {
            if(s_m_inputSystem != null)
                s_m_inputState.Update();
        }

        public void ChangeMap(int currentMap, int nextMap, Vector2 destination)
        {
            float posZ;
            AsyncOperation unloadOperation;
            AsyncOperation loadOperation;

            posZ = s_m_playerTransform.position.z;
            s_m_playerTransform.position = (Vector3)MetroidPhysics.c_TEMPORARY_PLAYER_GROUND_POSITION + Vector3.forward * posZ;

            unloadOperation = SceneManager.UnloadSceneAsync(currentMap);
            loadOperation = SceneManager.LoadSceneAsync(nextMap, LoadSceneMode.Additive);

            s_m_playerTransform.position = new Vector3(destination.x, destination.y, posZ);
        }
    }
}