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
    class RoomTagTypesUtils
    {
        public static List<RoomTagType> GetRoomTagType(ExternalCommandData commandData)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            List<RoomTagType> roomTagType = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_RoomTags)
                .Cast<RoomTagType>()
                .ToList();

            return roomTagType;
        }
    }
}
