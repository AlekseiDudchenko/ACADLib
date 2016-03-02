using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

using AAA = Autodesk.AutoCAD.ApplicationServices;

namespace ACADLib.Models
{
    public class Layers
    {

        public string NameLayer;
        public bool IsOffLayer;
        public Color LayerColor;
            
        /// <summary>
        /// Создает новый слой с указанным названием
        /// </summary>
        /// <param name="nameLayer"></param>
        public void CreateLayer(string nameLayer)
        {
            // Get the current document and database 
            Document acDoc = Application.DocumentManager.MdiActiveDocument;       
            Database acCurDb = acDoc.Database;
                       
            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Layer table for read 
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForRead) as LayerTable;
                
                //Если слоя с таким именем еще нет
                if (acLyrTbl.Has(nameLayer) == false)
                {
                    LayerTableRecord acLyrTblRec = new LayerTableRecord();
                    // Assign the layer the ACI color 1 and a name   
                    acLyrTblRec.Color = Color.FromColorIndex(ColorMethod.ByAci, 1);                    
                    acLyrTblRec.Name = nameLayer;

                    // Upgrade the Layer table for write                       
                    acLyrTbl.UpgradeOpen();
                    // Append the new layer to the Layer table and the transaction  
                    
                    acLyrTbl.Add(acLyrTblRec);
                    acTrans.AddNewlyCreatedDBObject(acLyrTblRec, true);
                }
                
                // Save the changes and dispose of the transaction 
                acTrans.Commit();
            }
        }
    


        /// <summary>
        /// Удаляет слой заданного имени
        /// </summary>
        /// <param name="nameLayer"></param>
        public void EraseLayer(string nameLayer)
        {
            // Получение документа и базы данных
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Старт транзакции
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Открыте слоя таблицы для записи
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId,
                                             OpenMode.ForRead) as LayerTable;

                //  Если есть слой с таким именем
                if (acLyrTbl.Has(nameLayer) == true)
                {
                    
                    ObjectIdCollection acObjIdColl = new ObjectIdCollection();

                    acObjIdColl.Add(acLyrTbl[nameLayer]);
                    acCurDb.Purge(acObjIdColl);

                    if (acObjIdColl.Count > 0)
                    {
                        //Открываем для записи
                        LayerTableRecord acLyrTblRec;
                        acLyrTblRec = acTrans.GetObject(acObjIdColl[0], OpenMode.ForWrite) as LayerTableRecord;

                        try
                        {
                            // Удаляем слой
                            acLyrTblRec.Erase(true);

                            // Сохраняем изменения и закрываем транзакцию
                            acTrans.Commit();
                        }
                        catch (Autodesk.AutoCAD.Runtime.Exception Ex)
                        {

                         // Сообщение об ошибке
                         Application.ShowAlertDialog("Error:\n" + Ex.Message);
                        }
                    }
                }
            }
        }

        

        /// <summary>
        /// Создает слой с заданными свойствами
        /// </summary>
        /// <param name="nLayer"></param>
        /// <param name="isOff"></param>
        /// <param name="color"></param>
        public void CreateLayer(string nLayer, bool isOff, Autodesk.AutoCAD.Colors.Color color)
        {
            // Получаем текущий документ и его БД
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Блокируем документ
            using (DocumentLock docloc = acDoc.LockDocument())
            {
                // Начинаем транзакцию
                using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
                {
                    // Открываем таблицу слоев документа
                    LayerTable acLyrTbl = tr.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite) as LayerTable;

                    // Если нет слоя с таким именем 
                    if (acLyrTbl.Has(nLayer) == false)
                    {
                        // создаем новый слой и устанавливаем параметры
                        LayerTableRecord acLyrTblRec = new LayerTableRecord();
                        acLyrTblRec.Name = nLayer; //имя
                        acLyrTblRec.IsOff = isOff; //видимость
                        acLyrTblRec.Color = color;  //цвет

                        // Заносим созданный слой в таблицу слоев
                        acLyrTbl.Add(acLyrTblRec);

                        // Добавляем созданный слой в документ
                        tr.AddNewlyCreatedDBObject(acLyrTblRec, true);
                    }
                    else
                        Application.ShowAlertDialog("Такой слой уже есть");

                    // фиксируем транзакцию
                    tr.Commit();
                }
            }
        }


        /// <summary>
        /// Изменение слоя
        /// </summary>
        /// <param name="nameLayer"></param>
        /// <param name="newNameLayer"></param>
        /// <param name="isOff"></param>
        /// <param name="colorLayer"></param>
        public void ChangeLayer(string nameLayer, string newNameLayer, bool isOff, Color colorLayer)  
        {
            // Получаем текущий документ и его БД
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Блокируем документ
            using (DocumentLock docloc = acDoc.LockDocument())
            {
                // Начинаем транзакцию
                using (Transaction tr = acCurDb.TransactionManager.StartTransaction())
                {
                    // Открываем таблицу слоев документа
                    LayerTable acLyrTbl = tr.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite) as LayerTable;
                    
                    // Получаем запись слоя для изменения
                    LayerTableRecord acLyrTblRec = tr.GetObject(acLyrTbl[nameLayer], OpenMode.ForWrite) as LayerTableRecord;
                    
                    // Задаем нужные параметры
                    // Если имя меняется
                    if (nameLayer != newNameLayer)
                        acLyrTblRec.Name = newNameLayer;
                    acLyrTblRec.IsOff = isOff; //видимость
                    acLyrTblRec.Color = colorLayer; //цвет
                    
                    // Фиксируем транзакцию
                    tr.Commit();
                }
            }
        }
    }
}