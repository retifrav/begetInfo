using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace begetInfo
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(
            object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e
            )
        {
            MessageBox.Show(
                //string.Format("Some error has just hapenned.{1}Details: {0}{1}Even more details: {2}", e.Exception.Message, Environment.NewLine, e.Exception.StackTrace),
                string.Format("Произошла неизвестная ошибка.{1}Подробности: {0}",
                    e.Exception.Message, Environment.NewLine),
                "Неизвестная ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error
                );
            e.Handled = true;
        }
    }
}
