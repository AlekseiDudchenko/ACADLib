
using System.Windows.Input;
using ACADLib.Models;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;

namespace ACADLib.ViewModel
{
    class LayerViewModel : ViewModelBase
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public LayerViewModel()
        {
            // Кнопка Создать слой
            ButtonCreateLayerCommand = new Command(arg => ButtonCreateLayer_ClickMethod());

            // Кнопка Применить для слоя
            BattonApplyLayerCommand = new Command(arg => ButtonApplyLayer_ClickMethod());

            // Кнопка Обновить
            GetLayerNameCommand = new Command(arg => GetLayerNameMethod());

        }

        // Список слоев
        private string[] _layerNameList;
        public string[] LayerNameList 
        { 
            get { return _layerNameList; }
            set
            {
                if (_layerNameList != value)
                {
                    _layerNameList = value;
                    OnPropertyChanged("LayerNameList");
                }

            }

        }

        // Цвет слоя 
        private Color _layerColor;
        public Color LayerColor
        {
            get { return _layerColor; }
            set
            {
                if (_layerColor != value)
                {
                    _layerColor = value;
                    OnPropertyChanged("LayerColor");
                }
            }
        }

        // Color Index
        private int _colorIndex;
        public int ColorIndex 
        {
            get { return _colorIndex; }
            set
            {
                if (_colorIndex != value)
                {
                    _colorIndex = value;
                    OnPropertyChanged("ColorIndex");
                }
            }
        }

        // Имя создаваемого или изменяемого слоя
        private string _layerName;
        public string LayerName
        {
            get { return _layerName; }
            set
            {
                if (_layerName != value)
                {
                    _layerName = value;
                    OnPropertyChanged("LayerName");
                }
            }
        }

        // Видимлсть слоя
        private bool _isOff;
        public bool IsOff
        {
            get { return _isOff; }

            set
            {
                if (_isOff != value)
                {
                    _isOff = value;
                    OnPropertyChanged("IsOff");
                }
            }
        }

        // Имя выбранного слоя
        private string _selectedLayer;
        public string SelectedLayer
        {
            get { return _selectedLayer; }
            set
            {
                if (_selectedLayer != value)
                {
                    _selectedLayer = value;
                    OnPropertyChanged("SelectedLayer");
                }
            }
        }
      
        #endregion


        #region Commands

        public ICommand ButtonCreateLayerCommand { get; set; }

        public ICommand BattonApplyLayerCommand { get; set; }

        public ICommand GetLayerNameCommand { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Нажатие на клавишу применить для Слоя
        /// </summary>
        private void ButtonApplyLayer_ClickMethod()
        {
            Layers newLayer = new Layers();

            // Получаем выбранный цвет в LayerColor
            GetColor();

            // Меняем слой
            newLayer.ChangeLayer(SelectedLayer, LayerName, !IsOff, LayerColor);

        }

        /// <summary>
        /// Нажатие на клавищу Создать для слоев. Создает новый слой с задаными свойствами.
        /// </summary>
        private void ButtonCreateLayer_ClickMethod()
        {
            // Если имя не пустое
            if (LayerName != "")
            {
                // Создаем новый слой c заданными в полях формы пораметрами
                var newLayer = new Layers();

                // Получаем выбранный цвет в LayerColor
                GetColor();

                //Создем новый слой с указанными параментами
                newLayer.CreateLayer(LayerName, !IsOff, LayerColor);
            }
        }


        /// <summary>
        /// Получает список слоев и записывает его в LayerNameList
        /// </summary>
        public void GetLayerNameMethod()
        {
            // Получаем текущий документ и его БД 
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            //!!Считаем, что больше 50 слоев использоваться не будет
            string[] layerNameArray = new string[50];

            // Начинаем транзакцию
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Открыаем новый слоя для записи
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead) as LayerTable;

                // счетсчик
                int i = 0;

                // Для каждого ObjectId из коллекции
                foreach (ObjectId acObjId in acLyrTbl)
                {
                    LayerTableRecord acLyrTblRec;
                    acLyrTblRec = acTrans.GetObject(acObjId, OpenMode.ForRead) as LayerTableRecord;

                    // Записываем оцередное имя слоя в массив
                    if (acLyrTblRec.Name != "")
                        layerNameArray[i] = acLyrTblRec.Name;

                    //Обнавляем индекс для массива
                    i += 1;
                }
                // DЗакрытие транзакции
            }
            //Функция возвращает список слоев в строковом массиве
            LayerNameList = layerNameArray;
        }

        #endregion

        /// <summary>
        /// Получаем настойки для слоя из формы в глобальные переменные
        /// </summary>
        void GetColor()
        {

            //получаем значение ComboBoxColor и выбираем соответствующий цвет 
            switch (ColorIndex)
            {
                case 0:
                    LayerColor = Color.FromRgb(255, 0, 0); // Красный
                    break;
                case 1:
                    LayerColor = Color.FromRgb(0, 255, 0);  // Зеленый
                    break;
                case 2:
                    LayerColor = Color.FromRgb(255, 255, 0);  // Желтый
                    break;
                case 3:
                    LayerColor = Color.FromRgb(0, 0, 255);  // Синий
                    break;
                case 4:
                    LayerColor = Color.FromRgb(255, 255, 255);  // Белый
                    break;
                case 5:
                    LayerColor = Color.FromRgb(0, 0, 0);  // Черный
                    break;
                case 6:
                    LayerColor = Color.FromRgb(192, 192, 192);  // Серый
                    break;
            }
         }    
    }

}

