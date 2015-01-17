using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;

namespace begetInfo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_sites_Click(object sender, RoutedEventArgs e)
        {
            // очистка предыдущих результатов
            tb_sites_rez.Clear();

            WebRequest webRequest = WebRequest.Create(
                Properties.Settings.Default.request_sites
                    .Replace("LOGINTEMPLATE", tb_login.Text.Trim())
                    .Replace("PASSWORDTEMPLATE", tb_password.Password.Trim())
                );
            WebResponse webResponse = webRequest.GetResponse();
            string answer = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

            // ответ beget.ru в формате JSON
            dynamic parsedJSON = JsonConvert.DeserializeObject(answer);

            // если статус ответа "ошибка"
            string status = ((string)parsedJSON.status).Trim();
            if (status != "success")
            {
                MessageBox.Show(
                    string.Format("Ответ с beget.ru:{0}{1}",
                        Environment.NewLine,
                        ((string)parsedJSON.error_text).Trim()
                        ),
                    "Ошибка при выполнении запроса",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
                return;
            }

            StringBuilder rez = new StringBuilder();

            // получаем доменное имя и ID каждого сайта
            foreach (var site in parsedJSON.answer.result)
            {
                rez.Append(string.Format("ID сайта: {0}{1}",
                    ((string)site.id).Trim(),
                    Environment.NewLine)
                    );
                rez.Append(string.Format("Доменное имя: {0}{1}",
                    ((string)site.fqdn).Trim(),
                    Environment.NewLine)
                    );
                rez.Append(Environment.NewLine);
            }

            tb_sites_rez.Text = rez.ToString();
        }
    }
}
