using System;
using UnityEngine;

[Serializable]
[ExecuteAlways]
[AddComponentMenu("Physics 2D/Elongated Hexagon Collider 2D")]
// 길쭉한 육각형
public class ElongatedHexagonCollider2D : MonoBehaviour
{
    private const float c_SQRT_2_DIV_2 = 0.707106781186547524f;

    public BoxCollider2D positive;
    public BoxCollider2D body;
    public BoxCollider2D negative;

    public float shortSize = 0.1f;
    [Range(1.0f, 32.0f)]
    public float bodyProportion = 5.0f;
    public m_Direction direction = m_Direction.Vertical;
    public Vector2 offset = Vector2.zero;

    private float shortSizeSqrt;
    private float longSize;
    private float bodyLongSize;
    private float bodyLongSizeHalf;
    private Vector2 tempVector2 = Vector2.zero;
    private Vector3 tempVector3 = Vector3.zero;

    public enum m_Direction : int
    {
        Vertical = 0,
        Horizontal = 1
    }

    void OnValidate()
    {
        Logic();
    }

    void Update()
    {
        Logic();
    }

    private void Logic()
    {
        if(body == null || positive == null || negative == null)
            return;

        SetProperties();

        if(direction == m_Direction.Vertical)
        {
            SetCollidersVertical();
            SetTransformsVertical();
        }
        else if(direction == m_Direction.Horizontal)
        {
            SetCollidersHorizontal();
            SetTransformsHorizontal();
        }
    }

    private void SetProperties()
    {
        shortSizeSqrt = shortSize * c_SQRT_2_DIV_2;
        longSize = bodyProportion * shortSize;
        bodyLongSize = longSize - shortSize;
        bodyLongSizeHalf = bodyLongSize * 0.5f;
    }

    private void SetCollidersVertical()
    {
        tempVector2.Set(shortSize, bodyLongSize);
        body.size = tempVector2;
        tempVector2.Set(shortSizeSqrt, shortSizeSqrt);
        positive.size = tempVector2;
        negative.size = tempVector2;
    }

    private void SetTransformsVertical()
    {
        tempVector2.Set(offset.x, offset.y);
        body.transform.localPosition = tempVector2;
        tempVector2.Set(offset.x, offset.y + bodyLongSizeHalf);
        positive.transform.localPosition = tempVector2;
        tempVector2.Set(offset.x, offset.y - bodyLongSizeHalf);
        negative.transform.localPosition = tempVector2;

        tempVector3.Set(0.0f, 0.0f, 0.0f);
        body.transform.eulerAngles = tempVector3;
        tempVector3.Set(0.0f, 0.0f, 45.0f);
        positive.transform.eulerAngles = tempVector3;
        negative.transform.eulerAngles = tempVector3;

        tempVector3.Set(1.0f, 1.0f, 1.0f);
        body.transform.localScale = tempVector3;
        positive.transform.localScale = tempVector3;
        negative.transform.localScale = tempVector3;
    }

    private void SetCollidersHorizontal()
    {
        tempVector2.Set(bodyLongSize, shortSize);
        body.size = tempVector2;
        tempVector2.Set(shortSizeSqrt, shortSizeSqrt);
        positive.size = tempVector2;
        negative.size = tempVector2;
    }

    private void SetTransformsHorizontal()
    {
        tempVector2.Set(offset.x, offset.y);
        body.transform.localPosition = tempVector2;
        tempVector2.Set(offset.x + bodyLongSizeHalf, offset.y);
        positive.transform.localPosition = tempVector2;
        tempVector2.Set(offset.x - bodyLongSizeHalf, offset.y);
        negative.transform.localPosition = tempVector2;

        tempVector3.Set(0.0f, 0.0f, 0.0f);
        body.transform.eulerAngles = tempVector3;
        tempVector3.Set(0.0f, 0.0f, 45.0f);
        positive.transform.eulerAngles = tempVector3;
        negative.transform.eulerAngles = tempVector3;

        tempVector3.Set(1.0f, 1.0f, 1.0f);
        body.transform.localScale = tempVector3;
        positive.transform.localScale = tempVector3;
        negative.transform.localScale = tempVector3;
    }
}
