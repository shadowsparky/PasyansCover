using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

//TODO: Отделение логики от дизайна. Дизайн может обращаться к логике, логика не может обращаться к дизайну.

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
	    \brief Класс, который используется на странице игры
	    \author Евгений 
	    \version BETA 0.2.3
	    \date Ноябрь 2017 года
	    \details В этом классе содержатся все процедуры/функции, алгоритмы, которые необходимы для игры.
        \warning Поскольку это BETA версия, то любая из процедур/функций/констант/переменных может в любой момент исчезнуть, если разработчик посчитает это нужным.
        \todo Отделение логики от дизайна. Дизайн может обращаться к логике, логика не может обращаться к дизайну.
        \copyright Все права на этот класс принадлежат разработчикам из AVB inc. 
    */
    public sealed partial class GamePage : Page
    {
        private static bool WinnerModeCheck = false; ///< Переменная, которая используется в качестве облегчения игры для пользователей. Благодаря ей, пользователь сам решает для себя - прошел он игру или нет.  

        /*Константы*/
        private const int StartCordX = 10; ///< Координата Х по умолчанию слева. 
        private const int NullRow = 0; ///< Первый ряд карт. Поскольку это с#, то все начинается с 0, а не 1. 
        private const int OneRow = 10; ///< Второй ряд карт. Поскольку это с#, то все начинается с 0, а не 1. 
        private const int TwoRow = 20; ///< Третий ряд карт. Поскольку это с#, то все начинается с 0, а не 1. 
        private const int ThreeRow = 30; ///< Четвертый ряд карт. Поскольку это с#, то все начинается с 0, а не 1. 
        private const int DefaultCardWidth = 74; ///< Стандартная ширина карты. Она используется при динамическом создании карты.
        private const int DefaultCardHeight = 99; ///< Стандартная высота карты. Она используется при динамическом создании карты.
        private const int DefaultCardNumber = 36; ///< Количество карт по-умолчанию. Это версия игры на 36 карт, поэтому их 36.
        private const int DefaultArraySize = 40; ///< К количеству карт по-умолчанию ещё добавляются 4 пустые карты. Это полное кол-во карт, которое будет располагаться на странице.
        private const int DefaultEmptyCardValue = 99; ///< Пустая карта обозначается как 99. 
        private const int DefaultRowNumber = 4; ///< Количество рядов с картами внутри игры. Их 4. 
        private const int DefaultColNumber = 10; ///< Количество карт в одном ряде. Их всего 10. 
        private const int EndOneEptyCardPosition = 9; ///< Последняя карта в первом ряде.
        private const int EndTwoEptyCardPosition = 19; ///< Последняя карта во втором ряде.
        private const int EndThreeEptyCardPosition = 29; ///< Последняя карта в третьем ряде.
        private const int EndFourEptyCardPosition = 39; ///< Последяя карта в четвертом ряде. 
        private const int StandartCordYChange = 120; ///< Стандартное изменение координаты Y при расстановке карт. 
        private const int StandartCordXChange = 85; ///< Стандартное изменение координаты Х при расстановке карт. В новых обновлениях эта константа роли играет мало, т.к появился адаптивный дизайн.

        /*Константы для адаптивного дизайна*/
        private const int AdaptiveDesignKostil1 = 900; ///< "Костыль" для адаптивного дизайна. Благодаря этой координате первые карты слева получают возможность быть адаптивными. 
        private const int AdaptiveDesignKostil2 = 1128; ///< "Костыль" для адаптивного дизайна. Благодаря этой координате первые карты слева получают возможность быть адаптивными. 
        private const double AdaptiveDesignStandartMultiplication = 0.1; ///< Стандартное умножение для работы адаптивного дизайна. Текущий размер по ширине умножается на это число и карты начинают располагаться так, как нужно.  

        /*Тузы*/
        private const int ClubAce = 0; ///< Расположение туза треф в колоде по-умолчанию. 
        private const int HeartAce = 9; ///< Расположение туза черв в колоде по-умолчанию.
        private const int SpadeAce = 18; ///< Расположение туза пик в колоде по-умолчанию.
        private const int DiamondAce = 27; ///< Расположение туза бубен в колоде по-умолчанию.

        /*Переменные*/
        private bool CheckInitialization = false; ///< Проверка инициализации карт, чтобы их нельзя было делать адаптивными до того, как они появятся.
        private Image global_save; ///< Копия карты, которую пользователь начинает передвигать 
        private Image[] BestImageDataSaver = new Image[DefaultArraySize]; ///< Массив, хранящий все карты, которые имеются на странице. 
        private Image BestMagicCard; ///< Макет карты, с которым производится работа в процедуре. 
        private int[] BestMagicArrayForBestMagicCards = new int[DefaultArraySize]; ///< Массив, хранящий все значения карт, которые имеются на странице.
        private double CordX = 10; ///< Координата X на странице.
        private double CordY = 1; ///< Координата Y на странице.

        /*Привязка значения к Image (Attached Property)*/

        /// Переменная, которая позволяет привязывать значения карт прямо к IMAGE.  
        public static readonly DependencyProperty SetAndGetBestMagicCardImageValueProperty =
            DependencyProperty.RegisterAttached("Set", typeof(int), typeof(GamePage), new PropertyMetadata(false));

        /*!
            \brief Включение режима победителя
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Включение реализуется благодаря переменной WinnerModeCheck. 
        */
        public void SetTrueWinnerCheckMode()
        {
            WinnerModeCheck = true;
        }


        /*!
            \brief Выключение режима победителя
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Выключение реализуется благодаря переменной WinnerModeCheck. 
        */
        public void SetFalseWinnerCheckMode()
        {
            WinnerModeCheck = false;
        }


        /*!
            \brief Процедура, срабатывающая при загрузке страницы. 
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details При загрузке страницы карты расставляются по своим местам только благодаря данной процедуре. Без этой процедуры ничего бы этого не работало.
            \param sender Содержит в себе информацию, о том, что сделал пользователь. 
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.    
        */
        private async Task LoadCards()
        {
            var i = 0; var j = 0; int[] TMPArrayBest = new int[4];
            if (WinnerModeCheck)
                ENDGame.Visibility = Visibility.Visible;

            while (i != DefaultCardNumber)
            {
                BestMagicArrayForBestMagicCards[i] = i;
                i++;
            }

            ShuffleClass shuffleClass = new ShuffleClass();
            j = DefaultCardNumber; var count = 0; i = 0;

            while (j != DefaultArraySize)
            {
                TMPArrayBest[i] = BestMagicArrayForBestMagicCards[count];
                BestMagicArrayForBestMagicCards[count] = DefaultEmptyCardValue;
                BestMagicArrayForBestMagicCards[j] = TMPArrayBest[i];
                i++;
                count = count + OneRow;
                j++;
            }

            shuffleClass.Shuffle(BestMagicArrayForBestMagicCards);
            i = 0; count = 0;

            while (i != DefaultArraySize)
            {
                if (BestMagicArrayForBestMagicCards[i] == DefaultEmptyCardValue)
                {
                    var TMP = BestMagicArrayForBestMagicCards[count];
                    BestMagicArrayForBestMagicCards[count] = BestMagicArrayForBestMagicCards[i];
                    BestMagicArrayForBestMagicCards[i] = TMP;
                    count = count + OneRow;
                }
                i++;
            }


            i = 0;
            double AdaptiveCordXChange = this.ActualWidth * AdaptiveDesignStandartMultiplication;

        /*Появление карты*/
            while (i != DefaultArraySize)
            {
                BestMagicCard = new Image();
                BestMagicCard.Name = "BestMagicCardNum" + "-" + i;
                BestMagicCard.SetValue(SetAndGetBestMagicCardImageValueProperty, i);
                BestMagicCard.Width = DefaultCardWidth;
                BestMagicCard.Height = DefaultCardHeight;
                BestMagicCard.Visibility = Visibility.Collapsed;

                /*Выравнивание карты*/
                BestMagicCard.VerticalAlignment = VerticalAlignment.Top;
                BestMagicCard.HorizontalAlignment = HorizontalAlignment.Left;

                /*Расположение карт*/
                Thickness SetMarginInTheBestMagicCards = BestMagicCard.Margin;

                if ((i == NullRow) || (i == OneRow) || (i == TwoRow) || (i == ThreeRow))
                {
                    if (i != NullRow)
                        this.CordY = this.CordY + StandartCordYChange;
                    if (ActualWidth < AdaptiveDesignKostil1)
                        this.CordX = 1;
                    else if (ActualWidth < AdaptiveDesignKostil2)
                        this.CordX = AdaptiveCordXChange * AdaptiveDesignStandartMultiplication;
                    else
                        this.CordX = AdaptiveCordXChange * (AdaptiveDesignStandartMultiplication + AdaptiveDesignStandartMultiplication);
                }
                else if (i != NullRow)
                {
                    this.CordX = this.CordX + AdaptiveCordXChange;
                }

                SetMarginInTheBestMagicCards.Left = this.CordX;
                SetMarginInTheBestMagicCards.Top = this.CordY;

                BestMagicCard.Margin = SetMarginInTheBestMagicCards;
                /*Если значение массива будет 99, то карта будет Empty, а если не 99, то карта будет соответствовать значению карты*/
                var CardPath = "";
                if (BestMagicArrayForBestMagicCards[i] == DefaultEmptyCardValue)
                {
                    CardPath = "ms-appx:///Cards/Empty.jpg";
                    BestMagicCard.CanDrag = false;
                    BestMagicCard.AllowDrop = true;
                }
                else
                {
                    CardPath = "ms-appx:///Cards/" + BestMagicArrayForBestMagicCards[i] + ".jpg";
                    BestMagicCard.CanDrag = true;
                    BestMagicCard.AllowDrop = false;
                }

                BestMagicCard.Source = new BitmapImage(new Uri(CardPath));
                Grid grid = this.Content as Grid;
                BestMagicCard.DragStarting += SaveImageInside;
                BestMagicCard.DragOver += Image_DragOver;
                BestMagicCard.Drop += Image_Drop;
                BestImageDataSaver[i] = BestMagicCard;
                grid.Children.Add(BestMagicCard);

                /*Протест циклу for*/
                i++;
            }

            this.CheckInitialization = true;
            LoadingIndicator.Visibility = Visibility.Collapsed;
            for (i = 0; i < DefaultArraySize; i++)
            {
                 await Task.Run(() =>
                 {
                     var test = new MessageDialog(Task.CurrentId.ToString(), "test");
                     BestImageDataSaver[i].Visibility = Visibility.Visible;
                 });
            }
        }

        private async void Game_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCards();
        }


        /*!
            \brief Сохраняет данные, передаваемые пользователем в переменную. 
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Сохраняет данные от sender и передает их в переменную global_save. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь. 
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.    
        */
        private void SaveImageInside(object sender, RoutedEventArgs e)
        {
            global_save = ((Image)sender);
        }


        /*!
            \brief Событие происходит при наведении одной карты на другую. 
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details При наведении одной карты на другую, это событие добавит рядом с картами надпись "Попробоваь перенести". 
            \param sender Содержит в себе информацию, о том, что сделал пользователь. 
            \param e Представляет собой данные для событий перетаскивания. 
        */
        private void Image_DragOver(object sender, DragEventArgs e)
        {
            ResourceLoader loader = ResourceLoader.GetForCurrentView();
            e.AcceptedOperation = DataPackageOperation.Move;
            e.DragUIOverride.Caption = loader.GetString("Game_ChangeCard");
        }


        /*!
            \brief Проверка на окончание игры №1. 
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Основная процедура, для окончания. Проверяет что первая карта (туз) и все последующие за ней совпадают по алгоритму. 
            \param minN первая N (туз)
            \paran maxN последний N
            \param N Счётчик, который проверяет карты после туза. 
            \return Если карты не соответсвуют алгоритму, который используется в пасьянсе ковёр для конкретного ряда карт, то функция возвратит False, если соответсвует, то True. 
        */
        private bool CheckRows(int minN, int maxN, int N)
        {
            if (BestMagicArrayForBestMagicCards[N] == ClubAce)
            {
                for (int i = minN; i < maxN; i++)
                    if (BestMagicArrayForBestMagicCards[N] != i)
                        return false;
                    else
                        N++;
            }
            return true;
        }

        /*!
            \brief Проверка на окончание игры №2. 
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Функция, проверяющая полностью ряд карт, используя 1 функцию. 
            \param minN первая N (туз)
            \paran maxN последний N
            \param N значение проверяемого ряда карт. 
            \return Если карты не соответсвуют алгоритму, который используется в пасьянсе ковёр для конкретного ряда карт, то функция возвратит False, если соответсвует, то True. 
        */
        private bool CheckRowEx(int n)
        {
            if (!CheckRows(ClubAce, HeartAce, n))
            {
                return false;
            }

            else if (BestMagicArrayForBestMagicCards[n] == HeartAce)
            {
                if (!CheckRows(HeartAce, SpadeAce, n))
                    return false;
            }
            else if (BestMagicArrayForBestMagicCards[n] == SpadeAce)
            {
                if (!CheckRows(SpadeAce, DiamondAce, n))
                    return false;
            }
            else if (BestMagicArrayForBestMagicCards[n] == DiamondAce)
            {
                if (!CheckRows(DiamondAce, DefaultCardNumber, n))
                    return false;
            }

            else if (BestMagicArrayForBestMagicCards[EndOneEptyCardPosition] != DefaultEmptyCardValue)
                return false;

            else if (BestMagicArrayForBestMagicCards[EndTwoEptyCardPosition] != DefaultEmptyCardValue)
                return false;

            else if (BestMagicArrayForBestMagicCards[EndThreeEptyCardPosition] != DefaultEmptyCardValue)
                return false;

            else if (BestMagicArrayForBestMagicCards[EndFourEptyCardPosition] != DefaultEmptyCardValue)
                return false;

            return true;
        }


        /*!
            \brief Проверка на окончание игры №2. 
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Функция, проверяющая сразу 4 ряда карт, используя 1 функцию. 
            \param minN первая N (туз)
            \paran maxN последний N
            \param N значение проверяемого ряда карт. 
            \return Если карты не соответсвуют алгоритму, который используется в пасьянсе ковёр для конкретного ряда карт, то функция возвратит False, если соответсвует, то True. 
        */
        private bool CheckGameEnd()
        {
            if (!CheckRowEx(NullRow))
                return false;
            else if (!CheckRowEx(OneRow))
                return false;
            else if (!CheckRowEx(TwoRow))
                return false;
            else if (!CheckRowEx(ThreeRow))
                return false;
            else
                return true;
        }

        /*!
            \brief Событие, срабатывающее при переносе карты на другую. 
            \author Евгений 
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если переносимая карта будет отличаться от карты, стоящей слева от карты, которую заменяют на 1 единицу, то она переместится. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Предоставляет данные для событий перетаскивания.
        */
        private async void Image_Drop(object sender, DragEventArgs e)
        {
            Image Newimg = sender as Image;
            Image Previmg = global_save;
            var NewArrayCount = (int) Newimg.GetValue(SetAndGetBestMagicCardImageValueProperty);
            var PrevArrayCount = (int) Previmg.GetValue(SetAndGetBestMagicCardImageValueProperty);

            var NewArrayResult = BestMagicArrayForBestMagicCards[NewArrayCount];
            var PrevArrayResult = BestMagicArrayForBestMagicCards[PrevArrayCount];
            var TMPRES = NewArrayResult;
 
            if (((NewArrayCount == NullRow) || (NewArrayCount == OneRow) || (NewArrayCount == TwoRow) || (NewArrayCount == ThreeRow))
                && 
                (BestMagicArrayForBestMagicCards[NewArrayCount] == DefaultEmptyCardValue))
            {
                /*При перемещении карты, на пустую специальную карту*/
                if ((BestMagicArrayForBestMagicCards[PrevArrayCount] == ClubAce) || (BestMagicArrayForBestMagicCards[PrevArrayCount] == HeartAce) || 
                    (BestMagicArrayForBestMagicCards[PrevArrayCount] == SpadeAce) || (BestMagicArrayForBestMagicCards[PrevArrayCount] == DiamondAce))
                {
                    var PathPrevImg = Newimg.Source;
                    var NamePrevImg = Newimg.Name;
                    Newimg.Source = Previmg.Source;
                    Newimg.AllowDrop = false;
                    Newimg.CanDrag = false;
                    Previmg.Source = PathPrevImg;
                    Previmg.CanDrag = false;
                    Previmg.AllowDrop = true;

                    BestMagicArrayForBestMagicCards[NewArrayCount] = PrevArrayResult;
                    BestMagicArrayForBestMagicCards[PrevArrayCount] = TMPRES;
                }
            }
            else if (((NewArrayCount != NullRow) || (NewArrayCount != OneRow) || (NewArrayCount != TwoRow) || 
                (NewArrayCount != ThreeRow)) && (BestMagicArrayForBestMagicCards[NewArrayCount] == DefaultEmptyCardValue))
            {
                /*Алгоритм для всех остальных карт*/
                if ((BestMagicArrayForBestMagicCards[PrevArrayCount] != ClubAce) || (BestMagicArrayForBestMagicCards[PrevArrayCount] != HeartAce) 
                    || (BestMagicArrayForBestMagicCards[PrevArrayCount] != SpadeAce) || (BestMagicArrayForBestMagicCards[PrevArrayCount] != DiamondAce))
                {
                    if (BestMagicArrayForBestMagicCards[NewArrayCount - 1] == PrevArrayResult - 1)
                    {          
                        var PathPrevImg = Newimg.Source;
                        var NamePrevImg = Newimg.Name;
                        Newimg.Source = Previmg.Source;
                        Newimg.CanDrag = true;
                        Newimg.AllowDrop = false;
                        Previmg.Source = PathPrevImg;
                        Previmg.CanDrag = false;
                        Previmg.AllowDrop = true;

                        BestMagicArrayForBestMagicCards[NewArrayCount] = PrevArrayResult;
                        BestMagicArrayForBestMagicCards[PrevArrayCount] = TMPRES;
                    }
                }
            }

            /*Конец игры*/

            if (CheckGameEnd())
            {
                ResourceLoader loader = ResourceLoader.GetForCurrentView();
                var WIN = new MessageDialog(loader.GetString("Game_Winner"));
                await WIN.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }



            // TODO: Перемешивание карт (не обязательно) 
        }

        /*!
            \brief Инициализация страницы. 
            \author Microsoft
            \version BETA 0.2.3
            \date Ноябрь 2017 года
        */
        public GamePage()
        {
            this.InitializeComponent();
        }


        /*!
            \brief Перемещение в меню. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь нажмет на кнопку "перейти в меню", то эта процедура выведет его в меню. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.
        */
        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /*!
            \brief *Читы* Выигрыш происходит при нажатии на кнопку. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь включит режим победителя и нажмет на невидимую кнопку, то он сразу же выиграет. Все благодаря этой кнопке.  
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.
        */
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ResourceLoader loader = ResourceLoader.GetForCurrentView();
            var WIN = new MessageDialog(loader.GetString("Game_Winner"));
            await WIN.ShowAsync();

            this.Frame.Navigate(typeof(MainPage));
        }

        /*!
            \brief Процедура для адаптивного дизайна. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details В этой процедуре происходит смена расстояния между картами. 
            \param SizeResize значение ширины экрана на данный момент у пользователя.
        */
        private void Triggered(double SizeResize)
        {
            for (var i = 0; i < DefaultArraySize; i++)
            {
                BestImageDataSaver[i].Visibility = Windows.UI.Xaml.Visibility.Visible;
                Thickness SetMarginInTheBestMagicCards = BestImageDataSaver[i].Margin;
                if ((i == NullRow) || (i == OneRow) || (i == TwoRow) || (i == ThreeRow))
                {
                    if (i != NullRow)
                        this.CordY = this.CordY + StandartCordYChange;
                    if (ActualWidth < AdaptiveDesignKostil1)
                        this.CordX = 1;
                    else if (ActualWidth < AdaptiveDesignKostil2)
                        this.CordX = SizeResize * AdaptiveDesignStandartMultiplication;
                    else
                        this.CordX = SizeResize * (AdaptiveDesignStandartMultiplication + AdaptiveDesignStandartMultiplication);
                }
                else if (i != NullRow)
                {
                    this.CordX = this.CordX + SizeResize;
                }

                SetMarginInTheBestMagicCards.Left = this.CordX;
                SetMarginInTheBestMagicCards.Top = this.CordY;

                BestImageDataSaver[i].Margin = SetMarginInTheBestMagicCards;
            }
        }


        /*!
            \brief Событие изменения размера приложения. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Предоставляет данные, связанные с событием изменения размера.
        */
        private void Game_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (CheckInitialization)
            {
                double AdaptiveCordXChange = this.ActualWidth * AdaptiveDesignStandartMultiplication;

                if (BestOfTheBestOfTheBestOfTheBestTRIGGER.IsEnabled != true)
                {
                    for (int i = 0; i < DefaultArraySize; i++)
                        BestImageDataSaver[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                else
                {
                    this.CordX = StartCordX;
                    this.CordY = 1;

                    for (int i = 0; i < DefaultArraySize; i++)
                        BestImageDataSaver[i].Visibility = Windows.UI.Xaml.Visibility.Visible;
                    Triggered(AdaptiveCordXChange);
                }
            }
        }


        /*!
            \brief Начинание игры заново. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь нажмет на кнопку "Начать заново", то эта процедура перемешает все карты заново. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Содержит информацию о состоянии и данные события, связанные с маршрутизируемым событием.
        */
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage));
        }

        /*!
            \brief Процедура с рекламой. 
            \author Евгений
            \version BETA 0.2.3
            \date Ноябрь 2017 года
            \details Если пользователь нажмет на рекламу, то эта процедура открой сайт рекламодателя. 
            \param sender Содержит в себе информацию, о том, что сделал пользователь.
            \param e Предоставляет данные для этого события
        */
        private async void AdvertImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string uriToLaunch = @"https://shadowsparky.ru/";
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }

    public class Class1
    {
    }
}