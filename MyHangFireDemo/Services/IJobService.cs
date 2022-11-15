using System.Threading.Tasks;

namespace MyHangFireDemo.Services
{
    public interface IJobService
    {
        void FireAndForgetJob();
        void ReccuringJob();
        void DelayedJob();
        void ContinuationJob();
        Task CreateGetNewBookJob();
        Task CreateNewBookReportJob();
    }
}
