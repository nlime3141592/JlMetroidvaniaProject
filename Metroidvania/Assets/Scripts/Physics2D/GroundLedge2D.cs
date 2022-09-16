using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class GroundLedge2D : Ground2D
    {
        [Header("Ledge Settings")]
        public LedgePivot ledgePivot = LedgePivot.Center;
        public LedgeShape ledgeShape = LedgeShape.Circle;
        public bool syncAngleBySlope = true;
        [Range(0.01f, 3.0f)]
        public float ledgeSize = 0.1f;

        [Header("Gizmo Settings")]
        public bool gizmoEnable = true;
        public Color gizmoColor = Color.magenta;
        public float gizmoSize = 0.15f;

        private GroundJoint2D m_joint;
        private CircleCollider2D m_circle;
        private BoxCollider2D m_box;

        private float[] ledgePositionWeight = new float[]
        {
            -1.0f, 1.0f,
            0.0f, 1.0f,
            1.0f, 1.0f,
            -1.0f, 0.0f,
            0.0f, 0.0f,
            1.0f, 0.0f,
            -1.0f, -1.0f,
            0.0f, -1.0f,
            1.0f, -1.0f,
        };

        public enum LedgePivot
        {
            LeftTop, Top, RightTop, Left, Center, Right, LeftBottom, Bottom, RightBottom
        }

        public enum LedgeShape
        {
            Circle, Box
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            GroundJoint2D joint = GetComponentInParent<GroundJoint2D>();

            if(joint != null)
            {
                if(m_joint == null)
                    m_joint = joint;
                else if(m_joint != joint)
                    m_joint = joint;
            }
            else
            {
                m_joint = null;
            }

            CircleCollider2D circle = GetComponent<CircleCollider2D>();

            if(circle != null)
            {
                if(m_circle == null)
                    m_circle = circle;
                else if(m_circle != circle)
                    m_circle = circle;

                m_circle.isTrigger = true;
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

                m_box.isTrigger = true;
            }
            else
            {
                m_box = null;
            }
        }

        protected override void OnLogicUpdate()
        {
            if(!CanNavigate())
                return;

            m_circle.enabled = ledgeShape == LedgeShape.Circle;
            m_box.enabled = ledgeShape == LedgeShape.Box;

            m_circle.isTrigger = true;
            m_box.isTrigger = true;

            m_circle.radius = ledgeSize * 0.5f;
            m_box.size = Vector2.one * ledgeSize;

            SetPosition();
        }

        protected override bool CanNavigate()
        {
            bool base_canNavigate = base.CanNavigate();

            return base_canNavigate && m_box != null && m_circle != null && m_joint != null;
        }

        protected override void OnDrawGizmo()
        {
            if(!gizmoEnable)
                return;

            if(!CanNavigate())
                return;

            Gizmos.color = gizmoColor;

            if(ledgeShape == LedgeShape.Circle)
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, 0.0f), gizmoSize * 0.5f);
            else if(ledgeShape == LedgeShape.Box)
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(gizmoSize, gizmoSize, 0));
        }

        private void SetPosition()
        {
            int pivotIndex = (int)ledgePivot;
            float x = m_joint.boxSize.x / 2;
            float y = m_joint.boxSize.y / 2;
            float wx = ledgePositionWeight[pivotIndex * 2];
            float wy = ledgePositionWeight[pivotIndex * 2 + 1];

            if(syncAngleBySlope)
                transform.localPosition = new Vector2(x * wx, y * wy);
            else
                transform.position = m_joint.transform.position + (Vector3)(new Vector2(x * wx, y * wy));
        }
    }
}