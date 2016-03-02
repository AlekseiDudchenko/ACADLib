using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACADLib
{
    public class PointClass : ObjectClass
    {

        public Point3d pointPosition;
        //public ObjectId pointID = new ObjectId();


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
                    
                    // Создаем точку с заданными координатами
                    DBPoint acPoint = new DBPoint(new Point3d(X1, Y1, Z1));
                        
                    //Установка значений по умолчанию. (Здесь нужно будет копировать свойства старой линии) 
                    //acPoint.SetDatabaseDefaults(); 
                    
                    // Добавляем новый объект в таблицу
                    acBlkTblRec.AppendEntity(acPoint);
                    acTrans.AddNewlyCreatedDBObject(acPoint, true);
                    
                    // Ствойства отображения точки 
                    // http://docs.autodesk.com/ACD/2010/ENU/AutoCAD%20.NET%20Developer%27s%20Guide/index.html?url=WS1a9193826455f5ff2566ffd511ff6f8c7ca-415b.htm,topicNumber=d0e15219
                    acCurDb.Pdmode = 0;
                    acCurDb.Pdsize = 1;
                                    
                    // Закрываем транзакцию 
                    acTrans.Commit();
                }
            }
        }
    }
}
