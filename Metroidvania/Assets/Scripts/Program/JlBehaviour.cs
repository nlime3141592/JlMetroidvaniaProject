using UnityEngine;

namespace JlMetroidvaniaProject
{
    /// <summary>
    /// 본 JlMetroidvania 프로젝트를 개발하는 개발자는 새로운 컴포넌트를 개발하기 위해
    /// MonoBehaviour 클래스가 아닌, JlBehaviour 클래스를 상속할 것을 적극 권장합니다.
    /// 본 클래스는 MonoBehaviour 클래스에서 제공하지는 않지만 사용하면 유용한 기능을 구현합니다.
    /// 또한, 기존 유니티 엔진에서 제공하는 이벤트 함수 뿐만 아니라
    /// 프로젝트에 맞는 임의의 이벤트를 설계할 수 있습니다.
    /// </summary>
    public abstract class JlBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void LateUpdate()
        {
            
        }
    }
}