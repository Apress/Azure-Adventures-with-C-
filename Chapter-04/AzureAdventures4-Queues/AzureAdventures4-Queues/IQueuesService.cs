namespace AzureAdventures4_Queues
{
    public interface IQueuesService
    {
        void SendMessage(string message);
        void SendObjectMessage(ReportModel model);
    }
}