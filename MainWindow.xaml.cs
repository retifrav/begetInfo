using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            tb_login.Text = Properties.Settings.Default.login;
            tb_password.Password = Properties.Settings.Default.password;
            tb_siteID.Text = Properties.Settings.Default.defaultSiteID;

            this.Closing += windowClosing;
        }

        public void windowClosing(object sender, CancelEventArgs e)
        {
            // if (!checkBeforeExit()) { e.Cancel = true; }
        }

        private void btn_sites_Click(object sender, RoutedEventArgs e)
        {
            // очистка предыдущих результатов
            lv_sites_rez.Items.Clear();
            //tb_sites_rez.Clear();

            string answer = sendRequest(
                Properties.Settings.Default.request_sites
                    .Replace("LOGINTEMPLATE", tb_login.Text.Trim())
                    .Replace("PASSWORDTEMPLATE", tb_password.Password.Trim())
                    );

            // ответ beget.ru в формате JSON
            dynamic parsedJSON = JsonConvert.DeserializeObject(answer);
            // сообщения об ошибках выдаются внутри этого метода, так что достаточно просто выйти
            if (!checkAnswer(parsedJSON)) { return; }

            //StringBuilder rez = new StringBuilder();

            // получаем доменное имя и ID каждого сайта
            foreach (var site in parsedJSON.answer.result)
            {
                lv_sites_rez.Items.Add(
                    new SitesRezItem(((string)Enumerable.First(site.domains).fqdn).Trim(), ((string)site.id).Trim())
                    );
                
                //rez.Append(string.Format("ID сайта: {0}{1}",
                //    ((string)site.id).Trim(),
                //    Environment.NewLine)
                //    );
                //rez.Append(string.Format("Доменное имя: {0}{1}",
                //    ((string)site.fqdn).Trim(),
                //    Environment.NewLine)
                //    );
                //rez.Append(Environment.NewLine);
            }

            //tb_sites_rez.Text = rez.ToString();
        }

        private void btn_siteLoad_Click(object sender, RoutedEventArgs e)
        {
            // очистка предыдущих результатов
            lv_siteLoad_rez.Items.Clear();

            if (string.IsNullOrEmpty(tb_siteID.Text.Trim()))
            {
                MessageBox.Show(
                    "Вы не указали ID сайта. Узнать его можно на вкладке \"Ваши сайты\".",
                    "Не указан ID сайта",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return;
            }

            //string siteid = System.Web.HttpUtility.UrlPathEncode("{'site_id':" + tb_siteID.Text.Trim() + "}");

            //string req = Properties.Settings.Default.request_siteLoad
            //        .Replace("LOGINTEMPLATE", tb_login.Text.Trim())
            //        .Replace("PASSWORDTEMPLATE", tb_password.Password.Trim())
            //        .Replace("SITEIDTEMPLATE", tb_siteID.Text.Trim());

            string answer = sendRequest(
                Properties.Settings.Default.request_siteLoad
                    .Replace("LOGINTEMPLATE", tb_login.Text.Trim())
                    .Replace("PASSWORDTEMPLATE", tb_password.Password.Trim())
                    .Replace("SITEIDTEMPLATE", tb_siteID.Text.Trim())
                    );

            // ответ beget.ru в формате JSON
            dynamic parsedJSON = JsonConvert.DeserializeObject(answer);
            // сообщения об ошибках выдаются внутри этого метода, так что достаточно просто выйти
            if (!checkAnswer(parsedJSON)) { return; }

            // получаем нагрузку сайта по датам
            foreach (var day in parsedJSON.answer.result.days)
            {
                lv_siteLoad_rez.Items.Insert(
                    0,
                    new SiteLoadRezItem(((string)day.date).Trim(), ((string)day.value).Trim())
                    );
            }
        } 

        /// <summary>
        /// Окно "О программе"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showAbout(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private bool checkBeforeExit()
        {
            MessageBoxResult what2do = MessageBox.Show(
                "Завершить работу приложения?",
                "Завершение работы",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
                );
            if (what2do == MessageBoxResult.Yes) { return true; } else { return false; }
        }

        /// <summary>
        /// Выход из приложения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shutdownApp(object sender, RoutedEventArgs e)
        {
            if (checkBeforeExit())
            {
                App.Current.Shutdown(0);
            }
        }

        private void save_clicked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.login = tb_login.Text.Trim();
            Properties.Settings.Default.password= tb_password.Password.Trim();
            Properties.Settings.Default.defaultSiteID = tb_siteID.Text.Trim();
            
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Отправка запроса к API
        /// </summary>
        /// <param name="request">веб-запрос</param>
        /// <returns>ответ на запрос</returns>
        private string sendRequest(string request)
        {
            WebRequest webRequest = WebRequest.Create(request);
            WebResponse webResponse = webRequest.GetResponse();
            string answer = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

            return answer;
        }

        private bool checkAnswer(dynamic parsedJSON)
        {
            // проверяем выполнение запроса
            string status_request = ((string)parsedJSON.status).Trim();
            if (status_request != "success")
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
                return false;
            }
            
            // проверяем ответ
            string status_answer = ((string)parsedJSON.answer.status).Trim();
            if (status_answer != "success")
            {
                string errorText = string.Empty;
                //foreach (var error in parsedJSON.answer.errors)
                //{
                errorText = ((string)Enumerable.First(parsedJSON.answer.errors).error_text).Trim();
                //}
                
                MessageBox.Show(
                    string.Format("Сервер вернул ошибку в ответ на запрос. Сообщение: {0}.", errorText),
                    "Ошибка в ответе",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
                return false;
            }

            return true;
        }

        private void copyIDclicked(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(((SitesRezItem)lv_sites_rez.SelectedItem).id);
        }  
    }

    public class SitesRezItem
    {
        public SitesRezItem(string domenVal, string idVal)
        {
            this.domen = domenVal;
            this.id = idVal;
        }

        public string domen { get; set; }
        public string id { get; set; }
    }

    public class SiteLoadRezItem
    {
        public SiteLoadRezItem(string date, string load)
        {
            this.date = date;
            this.load = load;
        }

        public string date { get; set; }
        public string load { get; set; }
    }
}
