using System.Collections;
using System.ComponentModel;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;


using acad = Autodesk.AutoCAD.ApplicationServices;

namespace ACADLib.Models
{
    public class Objects //: INotifyPropertyChanged
    {
        
        //public Point3d LineStartPoint;
        //public Point3d LineEndPoint;
        //public ObjectId LineID = new ObjectId();

        public Point3d PointOne;
   

        //public double _circleRadius;
        //public Point3d _circleCenter;
        //public ObjectId circleID = new ObjectId();
        
        // По ID удаляется объект.
        public ObjectId SelectedObjectId; //НИГДЕ НЕ ИСПОЛЬЗУЕТСЯ!

        /// <summary>
        /// Выделить с экрана отрезок
        /// </summary>
        public void GetOneObject(int typeObject)
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
                        switch (typeObject)
                        {
                            case 1: //point
                                {
                                    DBPoint acEnt = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead) as DBPoint;
                                    //Points newPoints = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead);// as Points;

                                    PointOne = acEnt.Position;
                                    //  acEnt.Get

                                    //Получаем координаты и ObjectId
                                    //newPoint.PointPosition = acEnt.Position;
                                    //newPoint.PointID = acEnt.Id;        
                                } break;
                            case 2: //line
                                {
                                    Line acEnt = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead) as Line;

                                    LineStartPoint = acEnt.StartPoint;
                                    LineEndPoint = acEnt.EndPoint;
                                    LineID = acEnt.Id;
                                } break;
                            case 3: // circle
                                {
                                    Circle acEnt = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead) as Circle;

                                    //Записываем нужные аттрибуты в глобальные переменные
                                    _circleCenter = acEnt.Center;
                                    _circleRadius = acEnt.Radius;
                                    circleID = acEnt.ObjectId;
                                } break;
                        }
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
        /// Добавление нового объекта
        /// </summary>
        public void AddObject(int typeObject, Point3d firstPoint, Point3d secondPoint, double radius)
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

                    switch (typeObject)
                    {
                        case 1: //point
                        {
                            // Создаем точку с заданными координатами
                            DBPoint acPoint = new DBPoint(firstPoint);

                            // Добавляем новый объект в таблицу
                            acBlkTblRec.AppendEntity(acPoint);
                            acTrans.AddNewlyCreatedDBObject(acPoint, true);

                            // Ствойства отображения точки 
                            acCurDb.Pdmode = 0;
                            acCurDb.Pdsize = 1;
                        }
                        break;

                        case 2: // line
                        {
                            // Создание отрезка 
                            Line acLine = new Line(firstPoint, secondPoint);

                            // Установка для отрезка свойст по умолчанию.
                            acLine.SetDatabaseDefaults();

                            // Добавление нового объекта в запись таблицы блоков и в транзакцию
                            acBlkTblRec.AppendEntity(acLine);
                            acTrans.AddNewlyCreatedDBObject(acLine, true);
                        } 
                        break;

                        case 3: // circle
                        {
                            // добавляем окружность
                            Circle acCircle = new Circle();

                            // устанавливаем параметры созданного объекта
                            acCircle.SetDatabaseDefaults();
                            acCircle.Center = firstPoint;
                            acCircle.Radius = radius;

                            // добавляем созданный объект в пространство модели и в транзакцию
                            acBlkTblRec.AppendEntity(acCircle);
                            acTrans.AddNewlyCreatedDBObject(acCircle, true);
                        } 
                        break;
                    }
                   

                    // Сохранение нового объекта в базе данных
                    acTrans.Commit();
                }
            }
        }  


        /// <summary>
        /// Удаление объекта по заданному ObjectID
        /// </summary>
        /// <param name="selObjectID"></param>
        public void Delete(ObjectId selObjectID)
        {
            // Получение текущего документа и базы данных
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            // Блакируем документ. 
            //Иначе при вызове функциис кнопки аварийно завершается автокад. Из консоли не обязательно
            using (DocumentLock docLock = acDoc.LockDocument())
            {

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

                    //Стираем выбранный объект
                    Entity item = (Entity)acTrans.GetObject(selObjectID, OpenMode.ForWrite);
                    item.Erase();

                    // Закрытие транзакции
                    acTrans.Commit();
                }
            }
        }
    }
}
