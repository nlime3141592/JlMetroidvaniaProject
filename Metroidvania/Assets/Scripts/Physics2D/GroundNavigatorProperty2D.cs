using System;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [Serializable]
    public class GroundNavigatorProperty2D
    {
        // joint, plain, joint, plain, ... , joint, plain, joint
        
        public float m_thickness;
        public int m_count;
        public int m_jointCount;
        public int m_plainCount;
        public List<Vector2> m_positions;
        public List<float> m_eulerAngles;
        public List<Vector2> m_boxSizes;
        public List<bool> m_overlaps;

        public GroundNavigatorProperty2D()
        {
            m_thickness = 0.1f;
            m_count = 0;
            m_positions = new List<Vector2>();
            m_eulerAngles = new List<float>();
            m_boxSizes = new List<Vector2>();
            m_overlaps = new List<bool>();
        }

        public void Calculate(Transform joints, float thickness)
        {
            int jCount = joints.childCount;
            int pCount = jCount - 1;
            if(pCount < 0)
                pCount = 0;
            int tCount = jCount + pCount;
            m_thickness = thickness;

            m_positions.CheckCapacity(tCount);
            m_eulerAngles.CheckCapacity(tCount);
            m_boxSizes.CheckCapacity(tCount);
            m_overlaps.CheckCapacity(tCount);
            m_count = tCount;
            m_jointCount = jCount;
            m_plainCount = pCount;

            if(jCount == 0)
                return;

            m_positions[0] = joints.GetChild(0).position;
            m_eulerAngles[0] = 0;
            m_boxSizes[0] = Vector2.one * thickness;
            m_overlaps[0] = false;

            if(jCount == 1)
                return;

            for(int i = 0; i < jCount; i++)
                SetJointPosition(joints.GetChild(i).position, i);

            for(int i = 0; i < pCount; i++)
                SetPlainPosition(i);

            for(int i = 0; i < pCount; i++)
            {
                SetPlainAngle(i);
                SetPlainBox(thickness, i);
            }
            for(int i = 0; i < jCount; i++)
            {
                SetJointAngle(i, jCount);
                SetJointBox(thickness, jCount, i);
            }
            m_overlaps[1] = false;
            for(int i = 2; i < jCount + pCount; i++)
            {
                int rest = i % 2;
                m_overlaps[i] = false;

                for(int j = rest; j < i; j += 2)
                {
                    if(rest == 0)
                    {
                        m_overlaps[i] |= IsOverlapJoint(i, j);
                    }
                    else
                    {
                        m_overlaps[i] |= IsOverlapPlain(i, j);
                    }
                }
            }
        }

        private void SetJointPosition(Vector2 pos, int index)
        {
            m_positions[2 * index] = pos;
        }

        private void SetPlainPosition(int index)
        {
            Vector2 l_pos = m_positions[2 * index];
            Vector2 r_pos = m_positions[2 * index + 2];

            m_positions[2 * index + 1] = (l_pos + r_pos) * 0.5f;
        }

        // check_l: > 0
        // check_r: < m_count
        private void SetJointAngle(int index, int jCount)
        {
            float angle = 0;
            int weight = 2;

            if(index > 0)
            {
                weight++;
                angle += m_eulerAngles[2 * index - 1];
            }
            if(index < jCount - 1)
            {
                weight++;
                angle += m_eulerAngles[2 * index + 1];
            }
            m_eulerAngles[2 * index] = angle / (weight / 2);
        }

        private void SetPlainAngle(int index)
        {
            Vector2 l_pos = m_positions[2 * index];
            Vector2 r_pos = m_positions[2 * index + 2];

            Vector2 dir = r_pos - l_pos;
            float angle = Vector2.Angle(Vector2.right, dir);
            if(dir.y < 0) angle *= -1;

            m_eulerAngles[2 * index + 1] = angle;
        }

        private void SetJointBox(float thickness, int jCount, int index)
        {
            if(index == 0 || index == jCount - 1)
            {
                m_boxSizes[2 * index] = Vector2.one * thickness;
                return;
            }

            float l_angle = m_eulerAngles[2 * index - 1];
            float r_angle = m_eulerAngles[2 * index + 1];

            float angle = r_angle - l_angle;

            while(angle < -180) angle += 360;
            while(angle > +180) angle -= 360;
            if(angle < 0) angle *= -1;

            angle /= 2;
            angle *= Mathf.Deg2Rad;

            float sin = Mathf.Sin(angle);
            float cos = Mathf.Sqrt(1 - sin * sin);

            float x = thickness * sin;
            float y = thickness * cos;

            m_boxSizes[2 * index] = new Vector2(x, y);
        }

        private void SetPlainBox(float thickness, int index)
        {
            Vector2 l_pos = m_positions[2 * index];
            Vector2 r_pos = m_positions[2 * index + 2];

            Vector2 dir = r_pos - l_pos;
            float length = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);

            m_boxSizes[2 * index + 1] = new Vector2(length, thickness);
        }

        private bool IsOverlapJoint(int index_l, int index_r)
        {
            // int i = 2 * index_l;
            // int j = 2 * index_r;
            int i = index_l;
            int j = index_r;
            bool isOverlap = true;

            isOverlap &= m_positions[i] == m_positions[j];

            return isOverlap;
        }

        private bool IsOverlapPlain(int index_l, int index_r)
        {
            // int i = 2 * index_l + 1;
            // int j = 2 * index_r + 1;
            int i = index_l;
            int j = index_r;
            bool isOverlap = true;

            isOverlap &= m_positions[i] == m_positions[j];
            isOverlap &= m_eulerAngles[i] == m_eulerAngles[j];
            isOverlap &= m_boxSizes[i] == m_boxSizes[j];

            return isOverlap;
        }
    }
}