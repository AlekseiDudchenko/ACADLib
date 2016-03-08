using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;


using acad = Autodesk.AutoCAD.ApplicationServices;

namespace ACADLib.Models
{
    public class Circles : Objects
    {
 
        /// <summary>
        /// Добавление окружности на ну с заданными параметрами 
        /// </summary>
        /// <param name="point3d"></param>
        /// <param name="radius"></param>
        [CommandMethod("AddCircle")]
        public void AddCircle(Point3d point3d, Double radius)
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            using (DocumentLock docLock = acDoc.LockDocument())
            {
                Database acCurDb = acDoc.Database;

                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Open the Block table for read
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                        OpenMode.ForRead) as BlockTable;

                    // Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;
                    {
                        // добавляем окружность
                        Circle acCircle = new Circle();

                        // устанавливаем параметры созданного объекта
                        acCircle.SetDatabaseDefaults();
                        acCircle.Center = point3d;
                        acCircle.Radius = radius;

                        // добавляем созданный объект в пространство модели и в транзакцию
                        acBlkTblRec.AppendEntity(acCircle);
                        acTrans.AddNewlyCreatedDBObject(acCircle, true);

                    }
                    // фиксируем изменения
                    acTrans.Commit();
                }
            }
        }
        
    }
}
