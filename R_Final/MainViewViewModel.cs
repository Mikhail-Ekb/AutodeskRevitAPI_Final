using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutodeskRevitAPI_Final
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        // Свойства
        public DelegateCommand Command { get; }
        private Document _doc;

        public List<RoomTagType> RoomTegTypes { get; }/* = new List<RoomTagType>(null);*/
        public RoomTagType SelectedRoomTegType { get; set; }
        public List<Level> Levels { get; } /*= new List<Level>(null);*/
        public Level SelectedLevel { get; set; }
        public DelegateCommand SaveCommand { get; }      


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = _commandData.Application.ActiveUIDocument.Document;

            RoomTegTypes = RoomTagTypesUtils.GetRoomTagType(commandData);
            Levels = LevelUtils.GetLevels(commandData);                
            SaveCommand = new DelegateCommand(OnSaveCommand);            
        }
        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            PlanTopology planTopology = doc.get_PlanTopology(SelectedLevel);
            
            using (var ts = new Transaction(doc, "Создание листов"))
            {
                ts.Start("Размещение марок помещений");
                foreach (ElementId eid in planTopology.GetRoomIds())
                {
                    Room tmpRoom = doc.GetElement(eid) as Room;

                    if (doc.GetElement(tmpRoom.LevelId) != null && tmpRoom.Location != null)
                    {
                        LocationPoint locationPoint = tmpRoom.Location as LocationPoint;
                        UV point = new UV(locationPoint.Point.X, locationPoint.Point.Y);
                        RoomTag newTag = doc.Create.NewRoomTag(new LinkElementId(tmpRoom.Id), point, null);
                        newTag.RoomTagType = SelectedRoomTegType;

                        List<RoomTag> tagListInTheRoom = new List<RoomTag>(newTag.Room.Id.IntegerValue);                        
                        tagListInTheRoom.Add(newTag);
                    }
                }
                ts.Commit();
            }
            RaiseCloseRequest();
        }


        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
