using System;
using System.Collections.Generic;
using UnityEngine;

namespace JlMetroidvaniaProject.MapManagement
{
    [ExecuteInEditMode]
    public abstract class Ground2D : MonoBehaviour
    {
        protected GroundNavigator2D navigator => m_navigator;
        protected GroundNavigatorProperty2D properties => m_properties;

        private GroundNavigator2D m_navigator;
        private GroundNavigatorProperty2D m_properties;
        private bool m_initialized = false;

        private void Reset()
        {
            if(IsPlaying())
                return;

            OnInitialize();
            OnLogicUpdate();
        }

        private void Start()
        {
            if(IsPlaying())
                return;

            OnInitialize();
            OnLogicUpdate();
        }

        private void OnValidate()
        {
            if(IsPlaying())
                return;

            if(!m_initialized)
            {
                OnInitialize();
            }

            if(transform.hasChanged)
            {
                OnTransformChange();
            }

            OnLogicUpdate();
        }

        private void Update()
        {
            if(IsPlaying())
                return;

            if(transform.hasChanged)
            {
                OnInitialize();
                OnTransformChange();
            }

            OnLogicUpdate();
        }

        private void OnDrawGizmos()
        {
            OnDrawGizmo();
        }

        protected virtual void OnInitialize()
        {
            m_initialized = true;

            GroundNavigator2D nav = GetComponentInParent<GroundNavigator2D>();

            if(nav != null)
            {
                if(m_navigator == null)
                    m_navigator = nav;
                else if(nav != m_navigator)
                    m_navigator = nav;

                m_properties = m_navigator.GetProperties();
            }
        }

        protected virtual void OnTransformChange()
        {

        }

        protected virtual void OnLogicUpdate()
        {

        }

        protected virtual void OnDrawGizmo()
        {

        }

        private bool IsPlaying()
        {
            return Application.IsPlaying(this);
        }

        protected virtual bool CanNavigate()
        {
            return m_navigator != null;
        }
    }
}