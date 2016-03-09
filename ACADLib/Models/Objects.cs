using System.Collections;
using System.ComponentModel;
using ACADLib.ViewModel;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;


using acad = Autodesk.AutoCAD.ApplicationServices;

namespace ACADLib.Models
{
    public class Objects //: INotifyPropertyChanged
    {
        /// <summary>
        /// Тип объекта
        /// </summary>
        public enum TypeObject
        {
            Point,
            Line,
            Circle
        }
                   
    
        /// <summary>
        /// Позволяет выделить объект в AutoCAD мышкой
        /// </summary>
        public void GetOneObject(TypeObject typeObject)
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
                            case TypeObject.Point: 
                                {
                                    DBPoint acEnt = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead) as DBPoint;
                                    //Points newPoints = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead);// as Points;
                                   

                                    //Получаем координаты и ObjectId
                                    //PointPosition = acEnt.Position;
                                    //PointID = acEnt.Id;        
                                } break;
                            case TypeObject.Line: 
                                {
                                    Line acEnt = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead) as Line;

                                    //Получаем координаты и ObjectId
                                    //LineStartPoint = acEnt.StartPoint;
                                    //LineEndPoint = acEnt.EndPoint;
                                    //LineID = acEnt.Id;
                                } break;
                            case TypeObject.Circle: 
                                {
                                    Circle acEnt = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead) as Circle;

                                    //Получаем координаты и ObjectId
                                    //CircleCenter = acEnt.Center;
                                    //CircleRadius = acEnt.Radius;
                                    //circleID = acEnt.ObjectId;
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



        //TODO Сделать перегрузки для разных типов объктов
        /// <summary>
        /// Добавление нового объекта
        /// </summary>
        public void AddObject(TypeObject typeObject, Point3d firstPoint, Point3d secondPoint, double radius)
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
                        case TypeObject.Point: //point
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

                        case TypeObject.Line: // line
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

                        case TypeObject.Circle: // circle
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
