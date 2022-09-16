using System;

public class DiscreteParabolaGraph : DiscreteGraph
{
    public DiscreteParabolaGraph(int length) : base(length) {}

    protected override float ArithmeticFunction(float domain_x)
    {
        float l = length - 1;
        return domain_x * (2 * l - domain_x) / (l * l);
    }
}