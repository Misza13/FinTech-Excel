namespace FinTech
{
    using System.Threading.Tasks;
    using ExcelDna.Integration;

    public class FinTechAddIn : IExcelAddIn
    {
        public void AutoOpen()
        {
            Task.Run(() =>
            {
                DeribitFunctions.DeribitSocket.Start();
            });
        }

        public void AutoClose()
        {
        }
    }
}