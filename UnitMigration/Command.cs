#region Namespaces
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace UnitMigration
{
  [Transaction( TransactionMode.Manual )]
  public class Command : IExternalCommand
  {
    private const string _taskTitle = "Migrate units";

    private const string _fileFilter 
      = "All Revit files (*.rvt, *.rfa, *.rte, *.rft)|*.rvt;*.rfa;*.rte;*.rft";

    public Result Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      TaskDialogResult result = TaskDialog.Show( _taskTitle, 
        "Select source file for units copy...",
        TaskDialogCommonButtons.Ok 
        | TaskDialogCommonButtons.Cancel );

      if( result == TaskDialogResult.Cancel )
        return Result.Cancelled;

      FileOpenDialog fileOpen = new FileOpenDialog( 
        _fileFilter );

      if( fileOpen.Show() == ItemSelectionDialogResult.Canceled )
        return Result.Cancelled;

      ModelPath modelPath = fileOpen.GetSelectedModelPath();
      if( modelPath == null )
        return Result.Failed;

      Document originalDoc = commandData.Application
        .OpenAndActivateDocument( modelPath, 
        new OpenOptions(), false )?.Document;

      if( originalDoc == null )
        return Result.Failed;

      DisplayUnit originalDisplayUnits = originalDoc.DisplayUnitSystem;
      Units originalUnits = originalDoc.GetUnits();
      if( originalUnits == null )
        return Result.Failed;

      ///////////////////////////
      // target
      ///////////////////////////
      result = TaskDialog.Show( _taskTitle
        , string.Format(
          "You have selected a document in {0} format.\n"
          + "Select target folder to copy units to - "
          + "all files will be upgraded & overwritten!",
          originalDisplayUnits.ToString() )
        , TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel );

      if( result == TaskDialogResult.Cancel )
        return Result.Cancelled;

      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
      {
        ShowNewFolderButton = false,
      };
      DialogResult dialogResult = folderBrowserDialog.ShowDialog();
      if( dialogResult == DialogResult.Cancel )
        return Result.Cancelled;

      DirectoryInfo directoryInfo = new DirectoryInfo(
        folderBrowserDialog.SelectedPath );
      if( !directoryInfo.Exists )
        return Result.Failed;

      IEnumerable<FileInfo> files = directoryInfo.EnumerateFiles( "*",
        SearchOption.AllDirectories );
      foreach( FileInfo file in files )
      {
        Document doc = commandData.Application
      .OpenAndActivateDocument( file.FullName )?.Document;
        if( doc == null )
          return Result.Failed;

        if( originalDoc != null )
        {
          originalDoc.Close();
          originalDoc = null;
        }

        Transaction transaction = new Transaction( doc,
          "Copy units to target" );
        transaction.Start();
        doc.SetUnits( originalUnits );
        transaction.Commit();

        doc.Save();
        originalDoc = doc;
      }

      return Result.Succeeded;
    }
  }
}
