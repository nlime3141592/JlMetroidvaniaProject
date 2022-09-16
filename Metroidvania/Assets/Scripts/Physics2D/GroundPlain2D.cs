using System;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GroundPlain2D : Ground2D
    {
        [Header("Gizmo Settings")]
        public bool gizmoEnable = true;
        public Color gizmoColor = Color.yellow;

        private BoxCollider2D m_box;
        private Transform m_parent;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            BoxCollider2D box = GetComponent<BoxCollider2D>();

            if(box != null)
            {
                if(m_box == null)
                    m_box = box;
                else if(m_box != box)
                    m_box = box;
            }
            else
            {
                m_box = null;
            }

            Transform parent = transform.parent;

            if(parent != null)
            {
                if(m_parent == null)
                    m_parent = parent;
                else if(m_parent != parent)
                    m_parent = parent;
            }
            else
            {
                m_parent = null;
            }
        }

        protected override void OnLogicUpdate()
        {
            if(!CanNavigate())
                return;

            int siblingIdx = transform.GetSiblingIndex();
            int arrayIdx = 2 * siblingIdx + 1;

            if(siblingIdx < properties.m_plainCount)
            {
                transform.position = properties.m_positions[arrayIdx];
                transform.eulerAngles = Vector3.forward * properties.m_eulerAngles[arrayIdx];
                transform.localScale = Vector3.one;

                m_box.size = properties.m_boxSizes[arrayIdx];
            }
        }

        protected override bool CanNavigate()
        {
            bool base_canNavigate = base.CanNavigate();

            return base_canNavigate && m_box != null && navigator.GetPlainParent() == m_parent;
        }

        protected override void OnDrawGizmo()
        {
            // Debug.Log(string.Format("can navigate: {0}", CanNavigate()));
            if(!gizmoEnable)
                return;

            if(!CanNavigate())
                return;

            Gizmos.color = gizmoColor;

            Vector2 center = transform.position;
            float r = m_box.size.x * 0.5f;
            float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;

            Gizmos.DrawLine(center - dir, center + dir);
        }
    }
}