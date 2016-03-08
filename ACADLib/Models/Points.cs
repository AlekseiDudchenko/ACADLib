using System.ComponentModel;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;


namespace ACADLib.Models
{
    public class Points : Objects
    {
        private Point3d _pointPosition;
        public Point3d PointPosition
        {
            get { return _pointPosition; }
            set { _pointPosition = value; }
        }


        public ObjectId PointID = new ObjectId();


        public void GetPoint(DBPoint Point)
        {
            PointPosition = Point.Position;
            PointID = Point.Id; 
        }

        /*
        /// <summary>
        /// Ставит точку с заданными координатами
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="Z1"></param>
        public void AddPoint(double X1, double Y1, double Z1)
        {
            // Получаем документ
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            
            // Блокируем документ
            using (DocumentLock docLock = acDoc.LockDocument())
            {
                // Получаем базу данных
                Database acCurDb = acDoc.Database;
                
                // Начинаем транзакцию
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Открываем блок для чтения
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;
                    
                    // Открываем запись в таблице для записи
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;

                    {
                        // Создаем точку с заданными координатами
                        DBPoint acPoint = new DBPoint(new Point3d(X1, Y1, Z1));

                        //Установка значений по умолчанию. (Здесь нужно будет копировать свойства старой линии) 
                        //acPoint.SetDatabaseDefaults(); 

                        // Добавляем новый объект в таблицу
                        acBlkTblRec.AppendEntity(acPoint);
                        acTrans.AddNewlyCreatedDBObject(acPoint, true);

                        // Ствойства отображения точки 
                        acCurDb.Pdmode = 0;
                        acCurDb.Pdsize = 1;

                    }

                    // Закрываем транзакцию 
                    acTrans.Commit();
                }
            }
        }*/
    }
}
