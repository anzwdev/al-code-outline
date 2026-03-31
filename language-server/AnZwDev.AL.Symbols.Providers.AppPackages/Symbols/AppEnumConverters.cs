using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Symbols.Providers.AppPackages.Symbols
{
    internal static class AppEnumConverters
    {

        /*
    Add,
    AddFirst,
    AddLast,
    AddBefore,
    AddAfter,
    MoveFirst,
    MoveLast,
    MoveBefore,
    MoveAfter,
    Modify 
         */

        public static PageControlChangeKind IntToPageControlChangeKind(int value)
        {
            switch (value)
            {
                case 0: return PageControlChangeKind.Add;
                case 1: return PageControlChangeKind.AddFirst;
                case 2: return PageControlChangeKind.AddLast;
                case 3: return PageControlChangeKind.AddBefore;
                case 4: return PageControlChangeKind.AddAfter;
                case 5: return PageControlChangeKind.MoveFirst;
                case 6: return PageControlChangeKind.MoveLast;
                case 7: return PageControlChangeKind.MoveBefore;
                case 8: return PageControlChangeKind.MoveAfter;
                case 9: return PageControlChangeKind.Modify;
            }
            return PageControlChangeKind.Undefined;
        }

        public static PageActionChangeKind IntToPageActionChangeKind(int value)
        {
            switch (value)
            {
                case 0: return PageActionChangeKind.Add;
                case 1: return PageActionChangeKind.AddFirst;
                case 2: return PageActionChangeKind.AddLast;
                case 3: return PageActionChangeKind.AddBefore;
                case 4: return PageActionChangeKind.AddAfter;
                case 5: return PageActionChangeKind.MoveFirst;
                case 6: return PageActionChangeKind.MoveLast;
                case 7: return PageActionChangeKind.MoveBefore;
                case 8: return PageActionChangeKind.MoveAfter;
                case 9: return PageActionChangeKind.Modify;
            }
            return PageActionChangeKind.Undefined;
        }

        public static PageViewChangeKind IntToPageViewChangeKind(int value)
        {
            switch (value)
            {
                case 0: return PageViewChangeKind.Add;
                case 1: return PageViewChangeKind.AddFirst;
                case 2: return PageViewChangeKind.AddLast;
                case 3: return PageViewChangeKind.AddBefore;
                case 4: return PageViewChangeKind.AddAfter;
                case 5: return PageViewChangeKind.MoveFirst;
                case 6: return PageViewChangeKind.MoveLast;
                case 7: return PageViewChangeKind.MoveBefore;
                case 8: return PageViewChangeKind.MoveAfter;
                case 9: return PageViewChangeKind.Modify;
            }
            return PageViewChangeKind.Undefined;
        }

        /*
            Area,
            Group,
            Action,
            Separator,
            ActionRef,
            CustomAction,
            SystemAction,
            FileUploadAction
        */

        public static PageActionKind IntToPageActionKind(int value)
        {
            switch (value)
            {
                case 0: return PageActionKind.Area;
                case 1: return PageActionKind.Group;
                case 2: return PageActionKind.Action;
                case 3: return PageActionKind.Separator;
                case 4: return PageActionKind.ActionRef;
                case 5: return PageActionKind.CustomAction;
                case 6: return PageActionKind.SystemAction;
                case 7: return PageActionKind.FileUploadAction;
            }
            return PageActionKind.Undefined;
        }

        /*
            Area,
            Group,
            CueGroup,
            Repeater,
            Fixed,
            Grid,
            Part,
            SystemPart,
            Field,
            Label,
            UserControl,
            ChartPart
        */

        public static PageControlKind IntToPageControlKind(int value)
        {
            switch (value)
            {
                case 0: return PageControlKind.Area;
                case 1: return PageControlKind.Group;
                case 2: return PageControlKind.CueGroup;
                case 3: return PageControlKind.Repeater;
                case 4: return PageControlKind.Fixed;
                case 5: return PageControlKind.Grid;
                case 6: return PageControlKind.Part;
                case 7: return PageControlKind.SystemPart;
                case 8: return PageControlKind.Field;
                case 9: return PageControlKind.Label;
                case 10: return PageControlKind.UserControl;
                case 11: return PageControlKind.ChartPart;
            }
            return PageControlKind.Undefined;
        }

        /*
            TableData = 0,
            Table = 1,
            Report = 3,
            Codeunit = 5,
            Xmlport = 6,
            Page = 8,
            Query = 9,
            System = 10
        */

        public static ObjectKind PermissionObjectKindToObjectType(int value)
        {
            switch (value)
            {
                case 0: return ObjectKind.TableData;
                case 1: return ObjectKind.Table;
                case 3: return ObjectKind.Report;
                case 5: return ObjectKind.Codeunit;
                case 6: return ObjectKind.XmlPort;
                case 8: return ObjectKind.Page;
                case 9: return ObjectKind.Query;
                case 10: return ObjectKind.System;
            }
            return ObjectKind.Unknown;
        }

    }
}
