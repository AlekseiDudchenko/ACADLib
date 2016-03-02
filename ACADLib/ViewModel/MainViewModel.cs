using System.Windows.Input;
using Autodesk.AutoCAD.ApplicationServices;


namespace ACADLib.ViewModel
{
    class MainViewModel : ViewModelBase 
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainViewModel()
        {   
            // Кнопка Точка   
            PointWindowCommand = new Command(arg => PointWindowMethod());

            // Кнопка Линия
            LineWindowCommand = new Command(arg => LineWindowMethod());

            // Кнопка Окржность
            CircleWindowCommand = new Command(arg => CircleWindowMethod());
            
        }

        #endregion

        #region Commands

        public ICommand PointWindowCommand { get; set; }

        public ICommand LineWindowCommand { get; set; }

        public ICommand CircleWindowCommand { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Создает окно для работы с Point
        /// </summary>
        private void PointWindowMethod()
        {
            PointUserControl myUserControl = new PointUserControl();
            myUserControl.Width = 300;
            myUserControl.Height = 300;
            Application.ShowModalWindow(myUserControl);
        }

        /// <summary>
        /// Создает окно для работы с Line
        /// </summary>
        private void LineWindowMethod()
        {
            LineUserControl myUserControl = new LineUserControl();
            myUserControl.Width = 300;
            myUserControl.Height = 300;
            Application.ShowModalWindow(myUserControl);
        }

        /// <summary>
        /// Создает окно для работы с Circle
        /// </summary>
        private void CircleWindowMethod()
        {
            CircleUserControl myUserControl = new CircleUserControl();
            myUserControl.Width = 300;
            myUserControl.Height = 300;
            Application.ShowModalWindow(myUserControl);
        }

        #endregion
    }
}