using System;

public abstract class DiscreteGraph
{
    public int length { get; private set; }

    public float this[int index]
    {
        get => m_values[index];
    }

    private float[] m_values;

    public DiscreteGraph(int length)
    {
        int i;
        this.length = length;
        m_values = new float[length];

        for(i = 0; i < length; i++)
        {
            m_values[i] = ArithmeticFunction(i);
        }
    }

    // domain_x는 0 이상 length 미만의 실수를 입력받는다.
    protected abstract float ArithmeticFunction(float domain_x);
}