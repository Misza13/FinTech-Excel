namespace FinTech
{
    using System.Threading.Tasks;
    using ExcelDna.Integration;
    using ExcelDna.IntelliSense;

    public class FinTechAddIn : IExcelAddIn
    {
        public void AutoOpen()
        {
            IntelliSenseServer.Install();
            
            Task.Run(() =>
            {
                DeribitFunctions.DeribitSocket.Start();
            });
        }

        public void AutoClose()
        {
            IntelliSenseServer.Uninstall();
            
            //TODO: close socket
        }
    }
}