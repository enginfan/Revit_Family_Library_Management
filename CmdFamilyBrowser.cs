using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using TripleKill.Views;

namespace TripleKill
{
    [Transaction(TransactionMode.Manual)]
    public class CmdFamilyBrowser : IExternalCommand
    {
        private UIDocument _uiDoc;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiApplication = commandData.Application;
            _uiDoc = uiApplication.ActiveUIDocument;
            var doc = commandData.Application.ActiveUIDocument.Document;
            var familyDocument = doc.IsFamilyDocument;
            if (familyDocument)
            {
                TaskDialog.Show("提示", "族文档中不可用！");
                return Result.Cancelled;
            }
            try
            {
                var instance = new FamilyBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };
                instance.PlaceFamily += DoPlace;
                instance.LoadFamily += DoLoad;
                var _ = new WindowInteropHelper(instance)
                {
                    Owner = Process.GetCurrentProcess().MainWindowHandle
                };
                instance.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return Result.Failed;
            }
        }

        private void DoLoad(string path)
        {
            var doc = _uiDoc.Document;
            var tran = new Transaction(doc, "loadFamily");
            tran.Start();
            doc.LoadFamily(path);
            tran.Commit();
            MessageBox.Show("载入成功","BIMBOX");
        }

        private void DoPlace(string name)
        {

            var doc = _uiDoc.Document;
            var families = from element in new FilteredElementCollector(doc).OfClass(typeof(Family)) where element.Name == name select element;
            var family = families.Cast<Family>().FirstOrDefault();
            if (family == null)
            {
                MessageBox.Show("族还未载入！", "BIMBOX");
                return;
            }
            var symbolId = family.GetFamilySymbolIds().FirstOrDefault();

            var symbol = doc.GetElement(symbolId) as FamilySymbol;
            _uiDoc.PromptForFamilyInstancePlacement(symbol);

        }
    }
}
