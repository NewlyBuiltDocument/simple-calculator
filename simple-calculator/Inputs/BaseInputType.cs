namespace simple_calculator.Inputs;

/// <summary>
/// 输入处理基类
/// </summary>
/// <param name="calculator"></param>
public abstract class BaseInputType(Calculator calculator)
{
    public readonly Calculator calculator = calculator;

    public abstract string GeneratedNewExpression(string content);
}
