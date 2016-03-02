using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
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
using AAC = Autodesk.AutoCAD.Colors;



namespace ACADLib
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : Window
    {

        public UserControl1()
        {
            InitializeComponent();

            // Блокируем кнопки
            BattonCreatePoint.IsEnabled = false;
            BattonCreateLine.IsEnabled = false;
            BattonCreateCircle.IsEnabled = false;
            TBX1.IsEnabled = false;
            TBX2.IsEnabled = false;
            TBY1.IsEnabled = false;
            TBY2.IsEnabled = false;
            TBZ1.IsEnabled = false;
            TBZ2.IsEnabled = false;
            TBRadius.IsEnabled = false;

            //Обновляем значение ComboBoxLayer
            UpdateComboBoxLayer();          
        }
        
        /// <summary>
        /// Команда вызывающая диалоговое окно
        /// </summary>
        [CommandMethod("StartF")]
        public void StartF()
        {
            // создаем окно WPF
            UserControl1 myUserControl = new UserControl1();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalWindow(myUserControl);
        }
 

        /// <summary>
        /// Обновляет значение ComboBoxLayer
        /// </summary>
        void UpdateComboBoxLayer()
        {
            LayerClass nLCU = new LayerClass();

            ///получаем массив имен слоев
            string[] setLayer = new string[50];
            setLayer = nLCU.GetLayerName();

            ///Заполняем значения ComboBoxLayer
            foreach (string lN in setLayer)
            {
                if ((lN != null) && (!ComboBoxLayer.Items.Contains(lN)))
                    ComboBoxLayer.Items.Add(lN);
            }

            // Сбрасываем TextBox с именем выбранного слоя
            NameLayerTextBox.Text = "";
        }

        //ID выделяемых объектов
        ObjectId SelObjectID = new ObjectId(); 

        /// <summary>
        /// Нажатие на кнопку Выбрать точку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGetPoint_Click(object sender, RoutedEventArgs e)
        {
            //Получаем выбранный объект
            PointClass nPC = new PointClass();
            nPC.GetOneObject(1);

            //Задаем его параметры в текстбоксы
            TBX1.Text = Convert.ToString(nPC._pointPosition.X);
            TBY1.Text = Convert.ToString(nPC._pointPosition.Y);
            TBZ1.Text = Convert.ToString(nPC._pointPosition.Z);

            // Получаем ID точки
            SelObjectID = nPC.pointID;
            
            // Разблокируем кнопку Применить и блокируем остальные
            BattonCreatePoint.IsEnabled = true;
            BattonCreateLine.IsEnabled = false;
            BattonCreateCircle.IsEnabled = false;

            TBX1.IsEnabled = true;
            TBX2.IsEnabled = false;
            TBY1.IsEnabled = true;
            TBY2.IsEnabled = false;
            TBZ1.IsEnabled = true;
            TBZ2.IsEnabled = false;
            TBRadius.IsEnabled = false;
        }

        /// <summary>
        /// Нажатие на клавишу Применить для Точки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BattonCreatePoint_Click(object sender, RoutedEventArgs e)
        {
            // Создаем точку с координатами из текстбоксов
            PointClass nPC = new PointClass();

            nPC.AddPoint(Convert.ToDouble(TBX1.Text),
                         Convert.ToDouble(TBY1.Text),
                         Convert.ToDouble(TBZ1.Text));

            // Удаляем старую точку
            nPC.Delete(SelObjectID);

        }


        /// <summary>
        /// Нажатие на кнопку Выбрать линию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGetLine_Click(object sender, RoutedEventArgs e)
        {
            //Получаем выбранный объект
            LineClass nLC = new LineClass();
            nLC.GetOneObject(2);

            //Задаем его параметры в текстбоксы
            TBX1.Text = Convert.ToString(nLC._lineStartPoint.X);
            TBX2.Text = Convert.ToString(nLC._lineEndPoint.X);
            TBY1.Text = Convert.ToString(nLC._lineStartPoint.Y);
            TBY2.Text = Convert.ToString(nLC._lineEndPoint.Y);
            TBZ1.Text = Convert.ToString(nLC._lineStartPoint.Z);
            TBZ2.Text = Convert.ToString(nLC._lineEndPoint.Z);

            //Запоминем ObjectID выделенной линии
            SelObjectID = nLC.lineID;
            
            //Разблокируем кнопку Применить и блокируем остальные
            BattonCreateLine.IsEnabled = true;
            BattonCreatePoint.IsEnabled = false;
            BattonCreateCircle.IsEnabled = false;

            TBX1.IsEnabled = true;
            TBX2.IsEnabled = true;
            TBY1.IsEnabled = true;
            TBY2.IsEnabled = true;
            TBZ1.IsEnabled = true;
            TBZ2.IsEnabled = true;
            TBRadius.IsEnabled = false;

        }

        /// <summary>
        ///  Нажатие на кнопку Применить для Линии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BattonCreateLine_Click(object sender, RoutedEventArgs e)
        {
            LineClass nLC = new LineClass();

            //Получем координаты точек линии из формы
            Point3d fPoint = new Point3d(Convert.ToDouble(TBX1.Text),
                                         Convert.ToDouble(TBY1.Text),
                                         Convert.ToDouble(TBZ1.Text));

            Point3d lPoint = new Point3d(Convert.ToDouble(TBX2.Text),
                                         Convert.ToDouble(TBY2.Text),
                                         Convert.ToDouble(TBZ2.Text));

            //Создаем новую линию с кординатами из TextDox'ов
            nLC.AddLine(fPoint, lPoint);

            //Удаляем выбранную линию
            nLC.Delete(SelObjectID);
        }

        /// <summary>
        /// Нажатие на кнопку Выбрать окружность
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGetCircle_Click(object sender, RoutedEventArgs e)
        {
            //Получаем выбранный объект
            CircleClass nOC = new CircleClass();
            nOC.GetOneObject(3);

            //Задаем параметры в текстбоксы
            TBX1.Text = Convert.ToString(nOC._circleCenter.X);
            TBY1.Text = Convert.ToString(nOC._circleCenter.Y);
            TBZ1.Text = Convert.ToString(nOC._circleCenter.Z);
            TBRadius.Text = Convert.ToString(nOC._circleRadius);

            // Получаем ID окружности
            SelObjectID = nOC.circleID;

            // Разблокируем кнопку пременить и боркируем остальные
            BattonCreateCircle.IsEnabled = true;
            BattonCreatePoint.IsEnabled = false;
            BattonCreateLine.IsEnabled = false;

            TBX1.IsEnabled = true;
            TBX2.IsEnabled = false;
            TBY1.IsEnabled = true;
            TBY2.IsEnabled = false;
            TBZ1.IsEnabled = true;
            TBZ2.IsEnabled = false;
            TBRadius.IsEnabled = true;
        }

        /// <summary>
        /// Нажатие на кнопку Применить для Окружности
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BattonCreateCircle_Click(object sender, RoutedEventArgs e)
        {
            CircleClass nCС = new CircleClass();

            // Получаем координты центра из TextBox'ов
            Point3d centerPoint = new Point3d(Convert.ToDouble(TBX1.Text),
                                              Convert.ToDouble(TBY1.Text),
                                              Convert.ToDouble(TBZ1.Text));

            //Создаем новую окружность
            nCС.AddCircle(centerPoint, Convert.ToDouble(TBRadius.Text));
         
            //Удаляем (стурую) выбранную
            //nCС.DeleteLine(SelObjectID);
           nCС.Delete(SelObjectID);           
        } 


        /// <summary>
        /// Нажатие на клавищу Создать для слоев. Создает новый слой с задаными свойствами.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCreateLayer_Click(object sender, RoutedEventArgs e)
        {
            //Получаем настойки для слоя из формы в глобальные переменные
            GetSettingLayerFromForm();

            if (NameLayerTextBox.Text != "")
            {
                //рисуем новый слой c заданными в полях формы пораметрами
                LayerClass nLC = new LayerClass();
                nLC.CreateLayer(NameLayerTextBox.Text, isOff, acColors);

                // Обновляем список слоев в ComboBox
                UpdateComboBoxLayer();
            }
        }


        /// <summary>
        /// Нажатие на клавишу Применить для слоев. Меняет заданные свойства (!выбранного!) слоя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonApplyLayer_Click(object sender, RoutedEventArgs e)
        {
            //Получаем настойки для слоя из формы в глобальные переменные
            GetSettingLayerFromForm();

            LayerClass nLC = new LayerClass();
            nLC.ChangeLayer(layerNameSelected, NameLayerTextBox.Text, isOff, acColors);

            // Обновляем список слоев в ComboBox
            UpdateComboBoxLayer();
        }

        
       /* enum ACADColor
        {
            Red = AAC.Color.FromRgb(255, 0, 0),
            Green = AAC.Color.FromRgb(0, 255, 0),
            Yellow = AAC.Color.FromRgb(255, 255, 0),
            Blue = AAC.Color.FromRgb(0, 0, 255),
            White = AAC.Color.FromRgb(255, 255, 255),
            Black = AAC.Color.FromRgb(0, 0, 0),
            Grey = AAC.Color.FromRgb(192, 192, 192)
        };*/

        //переменная для передачи цвета слоя в функцию
        AAC.Color acColors = new AAC.Color();
        //Переменная отвечающая за видимость
        bool isOff;

        /// <summary>
        /// Получаем настойки для слоя из формы в глобальные переменные
        /// </summary>
        void GetSettingLayerFromForm()
        {
            //определяем состояние CheckBox Видимость
            isOff = Convert.ToBoolean(!CheckIsOff.IsChecked);



            //получаем значение ComboBoxColor и выбираем соответствующий цвет 
            switch (ComboBoxColor.SelectedIndex)
            {
                case 0:
                    acColors = AAC.Color.FromRgb(255, 0, 0);
                    break;
                case 1:
                    acColors = AAC.Color.FromRgb(0, 255, 0);  //зеленый
                    break;
                case 2:
                    acColors = AAC.Color.FromRgb(255, 255, 0);  //желтый
                    break;
                case 3:
                    acColors = AAC.Color.FromRgb(0, 0, 255);  //синий
                    break;
                case 4:
                    acColors = AAC.Color.FromRgb(255, 255, 255);  //белый
                    break;
                case 5:
                    acColors = AAC.Color.FromRgb(0, 0, 0);  //Black
                    break;
                case 6:
                    acColors = AAC.Color.FromRgb(192, 192, 192);  //серый
                    break;
            }
        }

        // Хранит выбранное в ComboBox имя слоя 
        string layerNameSelected;

        /// <summary>
        /// Событие выбора значения в ComboBox слоев
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxLayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Получем имя выделенного слоя
            layerNameSelected = Convert.ToString(ComboBoxLayer.SelectedItem);

            // Заполняем соответствующий текстбокс именем выделенного слоя
            NameLayerTextBox.Text = Convert.ToString(ComboBoxLayer.SelectedItem); 

            LayerClass nLC = new LayerClass();
            
            //получаем массив значений IsOff для слоев
            bool[] setIsOff;
            setIsOff = nLC.GetLayerIsOff();

            //CheckIsOff присваиваем значение isOff для выделенного слоя
            CheckIsOff.IsChecked = !(setIsOff[ComboBoxLayer.SelectedIndex]);            
        }                                    
    }
}
