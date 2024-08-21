﻿using System.Data.SQLite;

namespace simple_calculator
{
    /// <summary>
    /// 窗体类，需为第一个类以便设计器渲染
    /// </summary>
    public partial class CalForm : Form
    {
        //显示字符串
        public string display = "";
        
        public CalForm()
        {
            InitializeComponent();
        }

        private void CalForm_Load(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        /// <summary>
        /// 更新显示的内容
        /// </summary>
        private void UpdateDisplay()
        {
            DisplayText.Text = display;
        }
        //字符计数
        private static int Chshu(ReadOnlySpan<char> a, char b = ' ')
        {
            int c = 0;
            foreach (char d in a)
            {
                if (d == b) { ++c; }
            }
            return c;
        }
        private static int Chshu(ref string a, int b, int c, char d = ' ')
        {
            return Chshu(a.AsSpan(b, c), d);
        }
        //字符串转数字
        private static double Svtoty(ref string a, int b = 0, int Base = 10)
        {
            double d = 0.0, e = Base;
            int? f = null, g = a.Length;
            for (; b < g && (a[b] >= '0' && a[b] <= '9' || a[b] == '.'); ++b)
            {
                if (a[b] == '.')
                {
                    f = b;
                }
                else
                {
                    d *= e;
                    d += a[b] - '0';
                }
            }
            if (f != null)
            {
                d /= Math.Pow(e, (double)f);
            }
            return d;
        }

        //数与符号分离
        private static List<double> Zhuan(ref string str)
        {
            List<double> a = [];
            str = str.Replace(" ", null);
            char[] c = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'];
            for (int d = str.IndexOfAny(c), e = str.IndexNotOfAny(c, d); d != -1;)
            {
                a.Add(Svtoty(ref str, d));
                str = string.Concat(str.AsSpan(0, d), " ", e == -1 ? null : str.AsSpan(e));
                d = str.IndexOfAny(c);
                e = str.IndexNotOfAny(c, d);
            }
            return a;
        }
        //无括号部分计算
        private static void Sshu(ref string a, ref List<double> b, int c, int d)
        {
            char[] de = ['+', '-'], df = ['*', '/'];
            for (int e = a.IndexOfAny(de, c), f = a.IndexNotOfAny(de, e); e < d; e = a.IndexOfAny(de, e), f = a.IndexNotOfAny(de, e))
            {
                if (e != 0 && a[e - 1] == ' ')
                {
                    ++e; continue;
                }
                if (f >= d) break;
                b[Chshu(ref a, 0, e)] *= (Chshu(ref a, e, f - e, '-') % 2 != 0 ? -1 : 1);
                a = a.Remove(e, f - e);
                d -= f - e;
            }
            for (int e = a.IndexOfAny(df, c), f; e < d; a = a.Remove(e, 2), d -= 2, b.RemoveAt(f), e = a.IndexOfAny(df, e))
            {
                f = Chshu(ref a, 0, e);
                if (a[e] == '*')
                {
                    b[f - 1] *= b[f];
                }
                else
                {
                    b[f - 1] /= b[f];
                }
            }
            for (int e = a.IndexOfAny(de, c), f; e < d; a = a.Remove(e, 2), d -= 2, b.RemoveAt(f), e = a.IndexOfAny(de, e))
            {
                f = Chshu(ref a, 0, e);
                if (a[e] == '+')
                {
                    b[f - 1] += b[f];
                }
                else
                {
                    b[f - 1] -= b[f];
                }
            }
        }
        //核心计算，对括号处理
        public static double Calculate(string expression)
        {
            string b = expression;
            List<double> c = Zhuan(ref b);
            for (int d = b.IndexOf(')'), e, f; d != -1; d = b.IndexOf(')', e))//有括号时对括号的处理
            {
                e = b.LastIndexOf('(', d);
                f = Chshu(ref b, 0, d);
                if (e != -1)
                {
                    Sshu(ref b, ref c, e + 1, d);
                    b = b.Remove(e, 1).Remove(e + 1, 1);
                    f = Chshu(ref b, 0, e);
                    if (b.Length > e && b[e + 1] == ' ')
                    {
                        c[f] *= c[f + 1];
                        b = b.Remove(e, 1);
                        c.RemoveAt(f + 1);
                    }
                    if (e != 0 && b[e - 1] == ' ')
                    {
                        c[f - 1] *= c[f];
                        b = b.Remove(e, 1);
                        c.RemoveAt(f);
                    }
                }
                else
                {
                    if (d != 0)
                    {
                        if (b[d - 1] == ' ' && b[d + 1] == ' ')
                        {
                            c[f - 1] *= c[f];
                            b = b.Remove(d, 2);
                            c.RemoveAt(f);
                        }
                    }
                    else b = b.Remove(0, 1);
                    e = 0;
                }
            }
            if (b.Contains('('))
            {
                b = b.Remove(b.IndexOf('('), 1);
            }
            for (int d = b.IndexOf('('), e; d != -1;)
            {
                if (d != 0 && b.Length >= d && b[d - 1] == ' ' && b[d] == ' ')
                {
                    e = Chshu(ref b, 0, d);
                    c[e - 1] *= c[e];
                    c.RemoveAt(e);
                }
                d = b.IndexOf('(');
                if (d != -1)
                {
                    b = b.Remove(d, 1);
                }
            }
            Sshu(ref b, ref c, 0, b.Length);
            return c[0];
        }


        /// <summary>
        /// 按下等于时执行计算程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void BtnEqual_Click(object sender, EventArgs e)
        {
            string ans = Calculate(display).ToString();
            //string ans = "ans";
            display += "=";
            display += ans;
            UpdateDisplay();
            AddToHistory(display);
            display = "";//清空显示字符串，等待下一次输入
        }

        /// <summary>
        /// 将计算结果添加到历史记录
        /// </summary>
        /// <param name="result">上一次结果的字符串</param>
        private static void AddToHistory(string result)
        {
            using SQLiteConnection conn = new(Program.myConnectionString);
            conn.Open();
            SQLiteCommand cmd = new();
            string insertStr = $"INSERT INTO history (results) VALUES ('{result}');";
            cmd = new SQLiteCommand(insertStr, conn);
            try { cmd.ExecuteNonQuery(); }
            catch (SQLiteException)
            {
                MessageBox.Show("无法添加到历史！");
            }
        }

        /// <summary>
        /// 显示历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHistory_Click(object sender, EventArgs e)
        {
            HistoryForm historyForm = new();
            historyForm.ShowDialog();
        }

        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (display.Length > 0)
            {
                display = display.Remove(display.Length - 1);
                UpdateDisplay();
            }
        }

        /// <summary>
        /// 清空输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            display = "";
            UpdateDisplay();
        }

        private void Btn0_Click(object sender, EventArgs e)
        {
            display += "0";
            UpdateDisplay();
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            display += "1";
            UpdateDisplay();
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            display += "2";
            UpdateDisplay();
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            display += "3";
            UpdateDisplay();
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            display += "4";
            UpdateDisplay();
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            display += "5";
            UpdateDisplay();
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            display += "6";
            UpdateDisplay();
        }

        private void Btn7_Click(object sender, EventArgs e)
        {
            display += "7";
            UpdateDisplay();
        }

        private void Btn8_Click(object sender, EventArgs e)
        {
            display += "8";
            UpdateDisplay();
        }

        private void Btn9_Click(object sender, EventArgs e)
        {
            display += "9";
            UpdateDisplay();
        }

        private void BtnDot_Click(object sender, EventArgs e)
        {
            display += ".";
            UpdateDisplay();
        }

        private void BtnLBracket_Click(object sender, EventArgs e)
        {
            display += "(";
            UpdateDisplay();
        }

        private void BtnRBracket_Click(object sender, EventArgs e)
        {
            display += ")";
            UpdateDisplay();
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            display += "+";
            UpdateDisplay();
        }

        private void BtnMinus_Click(object sender, EventArgs e)
        {
            display += "-";
            UpdateDisplay();
        }

        private void BtnTimes_Click(object sender, EventArgs e)
        {
            display += "*";
            UpdateDisplay();
        }

        private void BtnDivide_Click(object sender, EventArgs e)
        {
            display += "/";
            UpdateDisplay();
        }

        private void BtnExp_Click(object sender, EventArgs e)
        {
            display += "^";
            UpdateDisplay();
        }

    }
    //字符串和IndexOfAny对应方法
    public static class StringExtend
    {
        public static int IndexNotOfAny(this string str, char[] anyOf, int startIndex)
        {
            for (int a = startIndex; a < str.Length; ++a)
            {
                char b = str[a];
                bool c = true;
                foreach (char d in anyOf)
                {
                    if (b == d)
                    {
                        c = false;
                        break;
                    }
                }
                if (c)
                {
                    return a;
                }
            }
            return -1;
        }
    }
}
