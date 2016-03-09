using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;


namespace ACADLib.Models
{
    public class Lines : Objects
    {
        private Point3d _lineStartPoint;

        public Point3d LineStartPoint
        {
            get { return _lineStartPoint; }
            set { _lineStartPoint = value; }
        }

        private Point3d _lineEndPoint;
        public Point3d LineEndPoint
        {
            get { return _lineEndPoint; }
            set { _lineEndPoint = value; }
        }


        public ObjectId LineID;


        /// <summary>
        /// Выделить с экрана отрезок
        /// </summary>
        public void GetLine()
        {
            var acDocE = Application.DocumentManager.MdiActiveDocument.Editor;
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                var promtResult = acDocE.GetEntity("Выберите Объект");

                try
                {
                    if (promtResult.Status == PromptStatus.OK)
                    {
                        Line acEnt = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead) as Line;
                        
                        _lineStartPoint = acEnt.StartPoint;
                        _lineEndPoint = acEnt.EndPoint;
                        LineID = acEnt.Id;
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception Ex)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Error:\n" + Ex.Message);
                }

                //Закрываем транзакцию
                acTrans.Commit();
            }
        }




        /// <summary>
        /// Добавление новой линии
        /// </summary>
        public void AddLine(Point3d firstPoint, Point3d secondPoint)
        {
            // Получение текущего документа
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
             
            // Блокируем документ           
            using (DocumentLock docLock = acDoc.LockDocument())
            {
                //Получение базы данных текущего документа
                Database acCurDb = acDoc.Database;

                // Старт транзакции
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Открытие таблицы Блоков для чтения
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;

                    // Открытие записи таблицы Блоков пространства Модели для записи
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;

                    {

                        // Создание отрезка 
                        Line acLine = new Line(firstPoint, secondPoint);

                        // Установка для отрезка свойст по умолчанию. Свойства надо подавать как аргументы в функцию 
                        acLine.SetDatabaseDefaults();

                        // Добавление нового объекта в запись таблицы блоков и в транзакцию
                        acBlkTblRec.AppendEntity(acLine);
                        acTrans.AddNewlyCreatedDBObject(acLine, true);

                    }

                    // Сохранение нового объекта в базе данных
                    acTrans.Commit();
                }
            }
        }    
    }
}





