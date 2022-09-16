using System;

public class DiscreteLinearGraph : DiscreteGraph
{
    public DiscreteLinearGraph(int length) : base(length) {}

    protected override float ArithmeticFunction(float domain_x)
    {
        return domain_x / (length - 1);
    }
}