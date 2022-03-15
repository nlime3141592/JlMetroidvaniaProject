using UnityEngine;

namespace JlMetroidvaniaProject.Dummy
{
    public class DummyProgramManager : MonoBehaviour
    {
        private static DummyProgramManager s_m_manager;
        private static DummyInputSystem s_m_isystem;

        private DummyInputSystem m_isystem;

        public static bool CanGetPM(out DummyProgramManager manager)
        {
            if(s_m_manager == null)
            {
                manager = null;
                return false;
            }
            else
            {
                manager = s_m_manager;
                return true;
            }
        }

        public static bool CanGetInputSystem(out DummyInputSystem isystem)
        {
            if(s_m_isystem == null)
            {
                isystem = null;
                return false;
            }
            else
            {
                isystem = s_m_isystem;
                return true;
            }
        }

        private void Awake()
        {
            if(s_m_manager == null)
            {
                s_m_manager = this;
                DontDestroyOnLoad(this);

                s_m_isystem = new DummyInputSystem();
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        private void Update()
        {
            if(CanGetInputSystem(out m_isystem))
            {
                m_isystem.CheckInputs();
            }
        }
    }
}