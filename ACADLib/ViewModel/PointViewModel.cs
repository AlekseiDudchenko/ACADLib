
using System.Windows.Input;
using ACADLib.Models;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;


namespace ACADLib.ViewModel
{
    class PointViewModel : ViewModelBase
    {      
        /// <summary>
        /// Constructor.
        /// </summary>
        public PointViewModel()
        {          
            // Кнопка Выбрать Точку
            ButtonGetPointCommand = new Command(arg => ButtonGetPoint_ClickMethod());
            
            // Кнопка Применить
            BattonApplyPointCommand = new Command(arg => BattonApplyPoint_ClickMethod());
                
        }

        private ObjectId _selObjectID;

        #region Coordinates
        
        private double _x1;
        private double _y1;
        private double _z1;

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
       
        #endregion


        #region Commands

        public ICommand ButtonGetPointCommand { get; set; }

        public ICommand BattonApplyPointCommand { get; set; }
     

        #endregion


        #region Methods

        /// <summary>
        /// Нажатие на кнопку Выбрать точку
        /// </summary>
        private void ButtonGetPoint_ClickMethod()
        {
            //Получаем выбранный объект
            Points newPoint = new Points();    
            //newPoint.GetOneObject(Objects.TypeObject.Point);
            newPoint.GetPoint();
          
            //Задаем его параметры в текстбоксы
            X1 = newPoint.PointPosition.X;
            Y1 = newPoint.PointPosition.Y;
            Z1 = newPoint.PointPosition.Z;        

            // Получаем ID точки
            _selObjectID = newPoint.PointID;
        }


        /// <summary>
        /// Нажатие на клавишу Применить для Точки
        /// </summary>
        private void BattonApplyPoint_ClickMethod()
        {           
            // Создаем точку с координатами из текстбоксов
            //Points newPoint = new Points();

            Objects newPointObjects = new Objects();

            newPointObjects.AddObject(1, new Point3d(X1,Y1,Z1), new Point3d(), 0);

            // Удаляем старую точку
            newPointObjects.Delete(_selObjectID); 
        }   
        #endregion
    }   
}
