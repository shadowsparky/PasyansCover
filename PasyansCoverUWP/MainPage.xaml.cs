using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


/*!
    \brief Пространство имён.
    \author Евгений 
    \version BETA 0.2.3
    \date Ноябрь 2017 года
    \details В этом пространстве имен содержится вся игра.
    \warning Поскольку это BETA версия, то любой класс, который находится в этом пространстве может в любой момент исчезнуть, если разработчик посчитает это нужным.
    \copyright Все права на это пространство имен принадлежат разработчикам из AVB inc. 
*/
namespace PasyansCoverUWP
{
    /*!
        \brief Класс, который используется для перемещения по главному.
        \author Евгений 
        \version BETA 0.2.3
        \date Ноябрь 2017 года
        \details В этом классе содержатся события, которые необходимы для перемещения по меню.
        \warning Поскольку это BETA версия, то любая из процедур/функций/констант/переменных может в любой момент исчезнуть, если разработчик посчитает это нужным.
        \copyright Все права на этот класс принадлежат разработчикам из AVB inc. 
    */
    public sealed partial class MainPage : Page
    {
        /*!
            \brief Инициализация страницы. 
            \author Microsoft
            \version BETA 0.2.3
            \date Ноябрь 2017 года
        */
        public MainPage()
        {
            this.InitializeComponent();
        }

        /*!
            \brief Перемещение непосредствено в игру. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь нажмет на кнопку "Начать игру", то эта процедура выведет его на страницу с игрой. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.
        */
        private void GoGame_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage)); 
        }

        /*!
            \brief Перемещение в настройки. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь нажмет на кнопку "Настройки", то эта процедура выведет его в настройки. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.
        */
        private void GoSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }


        /*!
            \brief Перемещение на сайт с пожертвованиями. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь нажмет на кнопку "Пожертвовать", то эта процедура выведет его на сайт с пожертвованиями для разработчика. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.
        */
        private async void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceLoader loader = ResourceLoader.GetForCurrentView();
            var WIN = new MessageDialog(loader.GetString("Menu_DonateButtonMessage"));
            await WIN.ShowAsync();
            string uriToLaunch = @"https://shadowsparky.ru/about_us/";
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        /*!
            \brief Перемещение на сайт. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь нажмет на кнопку "Перейти на сайт", то эта процедура выведет его на сайт разработчика. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.
        */
        private async void GoToSiteButton_Click(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = @"https://shadowsparky.ru/";
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
