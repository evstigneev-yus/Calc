using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public static class CommonUtil
    {
        public static T Pop<T>(this IList<T> list)
        {
            var res = list.Last();
            list.RemoveAt(list.Count-1);
            return res;
        }
    }

/// <summary>
/// Логика взаимодействия для App.xaml
/// </summary>
public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }
    }
}
