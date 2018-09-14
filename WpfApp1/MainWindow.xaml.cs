using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int braces_cnt;
        List<string> ops;
        List<double> vals;
        List<int> openBracesIndexList;
        double x;
        public MainWindow()
        {
            braces_cnt = 0;
            ops = new List<string>();
            vals = new List<double>();
            openBracesIndexList = new List<int>();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var str = formulaTextBox.Text.Split(' ').ToList();
            x = double.Parse(varTextBox.Text);
            for (int i = 0; i < str.Count; i++)
            {
                if (str[i].Equals("("))
                {
                    braces_cnt++;
                    openBracesIndexList.Add(i);
                    str = BracesSolver(str, i);
                }
            }
            foreach (var s in str)
            {
                if (s.Equals("+")) ops.Add(s);
                else if (s.Equals("-")) ops.Add(s);
                else if (s.Equals("*")) ops.Add(s);
                else if (s.Equals("/")) ops.Add(s);
                else if (s.Equals("^")) ops.Add(s);
                else vals.Add(s.Equals("x") ? x : double.Parse(s));
            }

            Calc();
            resultTextBox.Text = vals.Pop().ToString();
        }
        private List<string> BracesSolver(List<string> str, int index)
        {
            for (int i = index; i < str.Count; i++)
            {
                while (braces_cnt != 0 || i > str.Count)
                {
                    i++;
                    if (str[i].Equals("("))
                    {
                        braces_cnt++;
                        openBracesIndexList.Add(i);
                        str = BracesSolver(str, i);
                        i = index;
                    }
                    if (str[i].Equals(")"))
                    {
                        for (int j = openBracesIndexList.Last() + 1; j < i; j++)
                        {
                            if (str[j].Equals("+")) ops.Add(str[j]);
                            else if (str[j].Equals("-")) ops.Add(str[j]);
                            else if (str[j].Equals("*")) ops.Add(str[j]);
                            else if (str[j].Equals("/")) ops.Add(str[j]);
                            else if (str[j].Equals("^")) ops.Add(str[j]);
                            else
                            {
                                vals.Add(str[j].Equals("x") ? x : double.Parse(str[j]));
                            }
                        }
                        Calc();
                        str[openBracesIndexList.Last()] = vals.Pop().ToString();
                        str.RemoveRange(openBracesIndexList.Last() + 1, i - openBracesIndexList.Last());
                        i = openBracesIndexList.Last();
                        openBracesIndexList.RemoveAt(openBracesIndexList.Count - 1);
                        braces_cnt--;
                        break;
                    }
                }
            }
            return str;
        }

        private void Calc()
        {
            var ind = ops.IndexOf("^");
            while (ind != -1)
            {
                var v1 = vals.ElementAt(ind);
                var v2 = vals.ElementAt(ind + 1);
                vals[ind] = Math.Pow(v1, v2);
                vals.RemoveAt(ind + 1);
                ops.RemoveAt(ind);
                ind = ops.IndexOf("^");
            }
            ind = ops.IndexOf("*");
            while (ind != -1)
            {
                var v1 = vals.ElementAt(ind);
                var v2 = vals.ElementAt(ind + 1);
                vals[ind] = v1 * v2;
                vals.RemoveAt(ind + 1);
                ops.RemoveAt(ind);
                ind = ops.IndexOf("*");
            }
            ind = ops.IndexOf("/");
            while (ind != -1)
            {
                var v1 = vals.ElementAt(ind);
                var v2 = vals.ElementAt(ind + 1);
                vals[ind] = v1 / v2;
                vals.RemoveAt(ind + 1);
                ops.RemoveAt(ind);
                ind = ops.IndexOf("/");
            }
            while (ops.Any())
            {
                var op = ops.Pop();
                var v = vals.Pop();
                if (op.Equals("+")) v = vals.Pop() + v;
                else if (op.Equals("-")) v = vals.Pop() - v;
                vals.Add(v);
            }
        }
    }
}
