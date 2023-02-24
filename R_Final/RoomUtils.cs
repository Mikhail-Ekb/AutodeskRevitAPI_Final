using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutodeskRevitAPI_Final
{
    class RoomUtils
    {
        public static List<Room> GetRooms(ExternalCommandData commandData)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            List<Room> rooms = new FilteredElementCollector(doc)
                .OfClass(typeof(Room))
                .OfType<Room>()
                .ToList();

            return rooms;
        }
    }
}
