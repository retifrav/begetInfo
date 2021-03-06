﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace begetInfo
{
    /// <summary>
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            vers.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void openLink(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try { Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)); }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("По каким-то причинам ссылка не открылась.{0}Подробности: {1}",
                        Environment.NewLine, ex.Message),
                    "Ошибка при открытии ссылки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }

            e.Handled = true;
        }

        private void copyEmail(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(((Run)email.Inlines.FirstOrDefault()).Text);
            MessageBox.Show(
                "Адрес электрической почты был скопирован в буфер обмена.",
                "E-mail",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
        }
    }
}
