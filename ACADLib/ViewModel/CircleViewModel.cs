using System.Windows.Input;
using ACADLib.Models;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace ACADLib.ViewModel
{
    class CircleViewModel : ViewModelBase
    {
        #region Constructor

        public CircleViewModel()
        {  
            // Кнопка Применить 
            BattonApplyCircleCommand = new Command(arg  => BattonApplyCircle_ClickMethod());

            // Кнопка Выбрать
            ButtonGetCircleCommand = new Command(aeg => ButtonGetCircle_ClickMethod());
       
        }

        




        private ObjectId _selObjectID;

        private double _x1;
        public double X1
        {
            get { return _x1; }
            set
            {
                if (_x1 != value)
                {
                    _x1 = value;
                    OnPropertyChanged("X1");
                }
            }
        }

        private double _y1;
        public double Y1
        {
            get { return _y1; }
            set
            {
                if (_y1 != value)
                {
                    _y1 = value;
                    OnPropertyChanged("Y1");
                }
            }
        }

        private double _z1;
        public double Z1
        {
            get { return _z1; }

            set
            {
                if (_z1 != value)
                {
                    _z1 = value;
                    OnPropertyChanged("Z1");
                }
            }
        }

        private double _circleRadius;
        public double CircleRadius
        {
            get { return _circleRadius; }

            set
            {
                if (_circleRadius != value)
                {
                    _circleRadius = value;
                    OnPropertyChanged("CircleRadius");
                }
            }
        }

        #endregion

        #region Commands

        public ICommand ButtonGetCircleCommand { get; set; }

        public ICommand BattonApplyCircleCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Нажатие на кнопку Выбрать окружность
        /// </summary>
        private void ButtonGetCircle_ClickMethod()
        {
            
            //Получаем выбранный объект
            Circles NewCircle = new Circles();
            NewCircle.GetOneObject(3);

            //Задаем параметры 
            X1 = NewCircle._circleCenter.X;
            Y1 = NewCircle._circleCenter.Y;
            Z1 = NewCircle._circleCenter.Z;
            CircleRadius = NewCircle._circleRadius;

            // Получаем ID окружности
            _selObjectID = NewCircle.circleID;
            
        }

        /// <summary>
        /// Нажатие на кнопку Применить для Окружности
        /// </summary>
        private void BattonApplyCircle_ClickMethod()
        {

            Circles NewCircle = new Circles();

            // Получаем координты центра 
            Point3d centerPoint = new Point3d(X1,Y1,Z1);

            //Создаем новую окружность
            NewCircle.AddCircle(centerPoint, CircleRadius);

            //Удаляем (стурую) выбранную
            NewCircle.Delete(_selObjectID);
            
        }

        #endregion

    }
}
