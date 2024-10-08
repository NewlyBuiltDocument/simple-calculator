using simple_calculator.Forms;

namespace simple_calculator.Functions;

/// <summary>
/// 历史记录功能类
/// </summary>
/// <param name="calculator">计算器类</param>
public class HistoryFunction(Calculator calculator) : BaseFunctionType(calculator)
{
    /// <summary>
    /// 显示历史记录
    /// </summary>
    public override void Function()
    {
        HistoryForm historyForm = new();
        historyForm.ShowDialog();
    }

    public override void Function(string args)
    {
        throw new NotImplementedException();
    }
}
