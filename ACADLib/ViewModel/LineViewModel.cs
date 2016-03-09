
using System.Windows.Input;
using ACADLib.Models;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;


namespace ACADLib.ViewModel
{
    class LineViewModel : ViewModelBase
    {
         

        /// <summary>
        /// Constructor.
        /// </summary>
        public LineViewModel()
        {
            // Нажатие на кнопку Выбрать линию
            ButtonGetLineCommand = new Command(arg => ButtonGetLine_ClicMethod());

            // Нажате на кнопек Применить
            BattonApplyLineCommand = new Command(arg => ButtonApplyLine_ClickMethod());             
        }

        #region Сoordinates

        private double _x1;
        private double _y1;
        private double _z1;
        private double _x2;
        private double _y2;
        private double _z2;

        private ObjectId _selObjectID;

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

        public double X2
        {
            get { return _x2; }
            set
            {
                if (_x2 != value)
                {
                    _x2 = value;
                    OnPropertyChanged("X2");
                }
            }
        }

        public double Y2
        {
            get { return _y2; }
            set
            {
                if (_y2 != value)
                {
                    _y2 = value;
                    OnPropertyChanged("Y2");
                }
            }
        }

        public double Z2
        {
            get { return _z2; }

            set
            {
                if (_z2 != value)
                {
                    _z2 = value;
                    OnPropertyChanged("Z2");
                }
            }
        }

        #endregion

        #region Commands

        public ICommand ButtonGetLineCommand { get; set; }

        public ICommand BattonApplyLineCommand { get; set; }
     
        #endregion


        #region Methods

        /// <summary>
        /// Нажатие на кнопку Выбрать Линию
        /// </summary>
        private void ButtonGetLine_ClicMethod()
        {
   
            //Получаем выбранный объект
            Lines newLine = new Lines();
            //newLine.GetOneObject(Objects.TypeObject.Line);
            newLine.GetLine();

            //Задаем его параметры в текстбоксы
            X1 = newLine.LineStartPoint.X;          
            Y1 = newLine.LineStartPoint.Y;          
            Z1 = newLine.LineStartPoint.Z;           
            X2 = newLine.LineEndPoint.X;
            Y2 = newLine.LineEndPoint.Y;
            Z2 = newLine.LineEndPoint.Z;

            //Запоминем ObjectID выделенной линии
            _selObjectID = newLine.LineID;         
            
        }

        /// <summary>
        ///  Нажатие на кнопку Применить для Линии
        /// </summary>
        private void ButtonApplyLine_ClickMethod()
        {
            Lines newLine = new Lines();

            //Получем координаты точек линии
            Point3d fPoint = new Point3d(X1,Y1,Z1);

            Point3d lPoint = new Point3d(X2,Y2,Z2);

            //Создаем новую линию  
            newLine.AddLine(fPoint, lPoint);

            //Удаляем выбранную линию
            newLine.Delete(_selObjectID);
        }     
        #endregion   
    }
}
