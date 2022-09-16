using System;
using System.Collections.Generic;

using UnityEngine;

namespace JlMetroidvaniaProject.Maps
{
    public class MapNetwork
    {
        // 맵 이동 규칙 (Connection 규칙)
        // p~(m1, m2) 관계는 포탈 번호 p를 이용해 맵 m1과 m2 간 이동 가능함을 의미하고 m1 <= m2이고 x > 0, y > 0이고 순서쌍 (m1, m2)에 대해
        // (0, 0) : 오류
        // (-1, -1) : 오류
        // (x, x) : 오류
        // => m1 == m2이면 오류
        // 위 조건에서 m1 == m2인 경우를 제외했으므로, 아래 조건부터는 m1 < m2임이 성립한다.
        // (-1, 0) : 오류
        // (-1, x) : 현재 맵 번호가 x가 아니면 어디서든 x로 이동 가능
        // (0, x) : 오류
        // (x, y) : 현재 맵 번호가 x이면 y로, 현재 맵 번호가 y이면 x로 이동 가능
        // => m1 < -1이거나 m1 == 0이거나 m2 == 0이면 오류
        private int[] m_connections;

        internal MapNetwork(int count)
        {
            if(count < 1)
                throw new ArgumentException("Portal count should be positive integer.");

            m_connections = new int[count + count];
        }

        internal void SetConnection(int portal, int map1, int map2)
        {
            if(map1 > map2)
            {
                int t = map1;
                map1 = map2;
                map2 = t;
            }

            // TODO: Scene의 빌드 인덱스 최대값을 넘어가지 않도록 제어하는 구문을 추가할 필요가 있음.
            if(map1 == map2 || map1 < -1 || map1 == 0 || map2 == 0 /*|| b > max_scene_build_index*/)
                throw new ArgumentException("올바르지 않은 포탈-맵 연결 관계");

            m_connections[portal * 2] = map1;
            m_connections[portal * 2 + 1] = map2;
        }

        /// <summary>
        /// 포탈 번호와 포탈에 매칭된 맵 번호를 이용하여 다음 이동할 맵을 결정합니다.
        /// </summary>
        /// <returns>다음 맵 번호 또는 오류 코드(-1)</returns>
        public int GetMap(int portal, int map)
        {
            int m1 = m_connections[portal * 2];
            int m2 = m_connections[portal * 2 + 1];
            int m = -1;
            
            if(m1 == -1 && map != m2)
                m = m2;
            else if(m1 == map)
                m = m2;
            else if(m2 == map)
                m = m1;

            if(m == -1)
                throw new Exception("현재 포탈 번호에 현재 맵이 매칭되어 있지 않음.");

            return m;
        }
    }
}