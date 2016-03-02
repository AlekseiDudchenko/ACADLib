using Autodesk.AutoCAD.Runtime;
using System.Windows;


namespace ACADLib.Views
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class MainUserControl  : Window
    {
        public MainUserControl()
        {
            InitializeComponent();     
        }
        
        /// <summary>
        /// Команда вызывающая диалоговое окно
        /// </summary>
        [CommandMethod("StartF")]
        public void StartF()
        {
            // создаем окно WPF
            var myUserControl = new MainUserControl();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalWindow(myUserControl);
        }
    }
}
