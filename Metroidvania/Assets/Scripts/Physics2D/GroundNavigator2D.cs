using System;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [ExecuteInEditMode]
    public class GroundNavigator2D : MonoBehaviour
    {
        [Range(0.01f, 3.0f)]
        public float thickness = 0.1f;

        public Transform _plains;
        public Transform _joints;

        public GroundNavigatorProperty2D m_properties;

        private void Reset()
        {
            if(IsPlaying())
                return;

            if(m_properties == null)
                m_properties = new GroundNavigatorProperty2D();
        }

        private void Start()
        {
            if(IsPlaying())
                return;

            if(m_properties == null)
                m_properties = new GroundNavigatorProperty2D();
        }

        private void Update()
        {
            if(IsPlaying())
                return;

            if(!CanNavigate())
                return;

            m_properties.Calculate(_joints, thickness);
            CheckOverlaps();
        }

        private bool IsPlaying()
        {
            return Application.IsPlaying(this);
        }

        private bool CanNavigate()
        {
            return _joints != null && _plains != null;
        }

        private void CheckOverlaps()
        {
            for(int i = 0; i < _joints.childCount; i++)
            {
                int idx = 2 * i;
                _joints.GetChild(i).gameObject.SetActive(!m_properties.m_overlaps[idx]);
            }
            for(int i = 0; i < _plains.childCount; i++)
            {
                int idx = 2 * i + 1;
                if(i < m_properties.m_plainCount)
                    _plains.GetChild(i).gameObject.SetActive(!m_properties.m_overlaps[idx]);
                else
                    _plains.GetChild(i).gameObject.SetActive(false);
            }
        }

        public GroundNavigatorProperty2D GetProperties()
        {
            return m_properties;
        }

        public Transform GetJointParent()
        {
            return _joints;
        }

        public Transform GetPlainParent()
        {
            return _plains;
        }
    }
}