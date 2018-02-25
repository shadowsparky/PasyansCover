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
        \brief Класс, который используется для настройки игры.
        \author Евгений 
        \version BETA 0.2.3
        \date Ноябрь 2017 года
        \details В этом классе содержатся процедуры, которые необходимы настройки игры.
        \warning Поскольку это BETA версия, то любая из процедур/функций/констант/переменных может в любой момент исчезнуть, если разработчик посчитает это нужным.
        \copyright Все права на этот класс принадлежат разработчикам из AVB inc. 
    */
    public sealed partial class Settings : Page
    {
        /*!
            \brief Инициализация страницы. 
            \author Microsoft
            \version BETA 0.2.3
            \date Ноябрь 2017 года
        */
        public Settings()
        {
            this.InitializeComponent();
        }


        /*!
            \brief Перемещение в меню. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь нажмет на кнопку "вернуться в меню", то эта процедура выведет его в меню. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.
        */
        private void GoToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage GP = new GamePage();
            if (CheckWinnerMode.IsChecked == true)
                GP.SetTrueWinnerCheckMode();
            else
                GP.SetFalseWinnerCheckMode();
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
