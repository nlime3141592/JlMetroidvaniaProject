using System;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class GroundJoint2D : Ground2D
    {
        public Vector2 boxSize => m_box.size;

        [Header("Joint Settings")]
        public bool jointEnable = true;
        public JointShape jointShape = JointShape.Circle;
        public bool ledgeGrabAvailable = false;
        public bool syncAngleBySlope = true;
        public bool syncSizeBySlope = true;

        [Header("Gizmo Settings")]
        public bool gizmoEnable = true;
        public Color gizmoColor = Color.cyan;
        [Range(0.01f, 3.0f)] public float gizmoSize = 0.2f;

        private CircleCollider2D m_circle;
        private BoxCollider2D m_box;
        private Transform m_parent;

        public enum JointShape
        {
            Circle, Box
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            CircleCollider2D circle = GetComponent<CircleCollider2D>();

            if(circle != null)
            {
                if(m_circle == null)
                    m_circle = circle;
                else if(m_circle != circle)
                    m_circle = circle;
            }
            else
            {
                m_circle = null;
            }

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

            // update transform informations.
            int siblingIdx = transform.GetSiblingIndex();
            int arrayIdx = 2 * siblingIdx;

            if(siblingIdx < properties.m_jointCount)
            {
                if(syncAngleBySlope)
                    transform.eulerAngles = Vector3.forward * properties.m_eulerAngles[arrayIdx];
                else
                    transform.eulerAngles = Vector3.zero;
                transform.localScale = Vector3.one;

                m_circle.radius = properties.m_thickness * 0.5f;
                if(syncSizeBySlope)
                    m_box.size = properties.m_boxSizes[arrayIdx];
                else
                    m_box.size = Vector2.one * properties.m_thickness;

                m_circle.enabled = jointShape == JointShape.Circle && jointEnable;
                m_box.enabled = jointShape == JointShape.Box && jointEnable;
            }

            // update ledge point.
        }

        protected override bool CanNavigate()
        {
            bool base_canNavigate = base.CanNavigate();

            return base_canNavigate && m_box != null && m_circle != null && navigator.GetJointParent() == m_parent;
        }

        protected override void OnDrawGizmo()
        {
            if(!gizmoEnable)
                return;

            if(!CanNavigate())
                return;

            Gizmos.color = gizmoColor;

            if(jointShape == JointShape.Circle)
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0.0f), gizmoSize * 0.5f);
            else if(jointShape == JointShape.Box)
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(gizmoSize, gizmoSize, 0));
        }
    }
}