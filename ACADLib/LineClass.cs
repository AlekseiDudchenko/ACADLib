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
    public class LineClass : ObjectClass
    {

        public Point3d lineStartPoint, lineEndPoint;
        //public ObjectId lineID = new ObjectId();

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

                    // Создание отрезка 
                    Line acLine = new Line(firstPoint, secondPoint);

                    // Установка для отрезка свойст по умолчанию. Свойства надо подавать как аргументы в функцию 
                    acLine.SetDatabaseDefaults();

                    // Добавление нового объекта в запись таблицы блоков и в транзакцию
                    acBlkTblRec.AppendEntity(acLine);
                    acTrans.AddNewlyCreatedDBObject(acLine, true);
                    
                    // Сохранение нового объекта в базе данных
                    acTrans.Commit();
                }
            }
        }    
    }
}





