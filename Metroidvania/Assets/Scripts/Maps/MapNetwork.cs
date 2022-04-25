using System;
using System.Collections.Generic;

using UnityEngine;

namespace JlMetroidvaniaProject.Maps
{
    public class MapNetwork
    {
        // requires
        // 1. (Now Map, Portal) -> (Next Map), implemented by mapNetwork.
        // 2. (Map, Portal) -> (Next Map Destination x, y), implemented by destiTable.

        private static readonly InvalidOperationException m_c_MISSING_NETWORK_EXCEPTION = new InvalidOperationException("맵 및 포탈 연결 정보가 없습니다.");
        private static readonly InvalidOperationException m_c_PORTAL_COUNT_EXCEPTION = new InvalidOperationException("포탈 갯수는 양수여야 합니다.");
        private static readonly IndexOutOfRangeException m_c_MAP_INDEX_EXCEPTION = new IndexOutOfRangeException("맵 번호는 음수일 수 없습니다.");
        private static readonly IndexOutOfRangeException m_c_PORTAL_INDEX_EXCEPTION = new IndexOutOfRangeException("포탈 번호는 음수일 수 없습니다.");

        // NOTE: 실제 코드에 포함시킬 때 public을 private으로 변경하기.
        public int m_portalCount;
        public int[,] m_mapNetwork;
        public float[,] m_destiTable;

        public MapNetwork(int portalCount)
        {
            if(portalCount < 1)                     throw m_c_PORTAL_COUNT_EXCEPTION;
            
            m_portalCount = portalCount;
            m_mapNetwork = new int[2, portalCount];
            m_destiTable = new float[4, portalCount];

            Clear();
        }

        public int GetNextMap(int currentMap, int portal)
        {
            if(!s_m_isInvalidMap(currentMap))       throw m_c_MAP_INDEX_EXCEPTION;
            else if(!s_m_isInvalidPortal(portal))   throw m_c_PORTAL_INDEX_EXCEPTION;

            if(m_mapNetwork[0, portal] == currentMap)
            {
                return m_mapNetwork[1, portal];
            }
            else if(m_mapNetwork[1, portal] == currentMap)
            {
                return m_mapNetwork[0, portal];
            }
            else
            {
                throw m_c_MISSING_NETWORK_EXCEPTION;
            }
        }

        public void GetDestination(int map, int portal, out float x, out float y)
        {
            if(!s_m_isInvalidMap(map))              throw m_c_MAP_INDEX_EXCEPTION;
            else if(!s_m_isInvalidPortal(portal))   throw m_c_PORTAL_INDEX_EXCEPTION;

            if(m_mapNetwork[0, portal] == map)
            {
                x = m_destiTable[0, portal];
                y = m_destiTable[1, portal];
            }
            else if(m_mapNetwork[1, portal] == map)
            {
                x = m_destiTable[2, portal];
                y = m_destiTable[3, portal];
            }
            else
            {
                throw m_c_MISSING_NETWORK_EXCEPTION;
            }
        }

        public void Clear()
        {
            int i;
            int j;

            for(i = 0; i < 2; i++)
            for(j = 0; j < m_portalCount; j++)
            {
                m_mapNetwork[i, j] = -1;
                m_destiTable[2 * i + 0, j] = 0.0f;
                m_destiTable[2 * i + 1, j] = 0.0f;
            }
        }

        private static bool s_m_isInvalidMap(int map) => map >= 0;
        private static bool s_m_isInvalidPortal(int portal) => portal >= 0;
    }
}