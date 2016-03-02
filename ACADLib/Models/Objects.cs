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
        
        public Point3d _lineStartPoint, _lineEndPoint;
        public ObjectId lineID = new ObjectId();

        public Point3d _pointPosition;
        public ObjectId pointID = new ObjectId();

        public double _circleRadius;
        public Point3d _circleCenter;
        public ObjectId circleID = new ObjectId();
        
        public ObjectId SelectedObjectId;

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

                                    //Получаем координаты и ObjectId
                                    _pointPosition = acEnt.Position;
                                    pointID = acEnt.Id;        
                                } break;
                            case 2: //line
                                {
                                    Line acEnt = acTrans.GetObject(promtResult.ObjectId, OpenMode.ForRead) as Line;

                                    _lineStartPoint = acEnt.StartPoint;
                                    _lineEndPoint = acEnt.EndPoint;
                                    lineID = acEnt.Id;
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
