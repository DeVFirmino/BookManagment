 
using System.Data;

namespace BooksApi.Services.ReportService
{
    public interface IReportInterface
    {
        DataTable CollectData<T>(List<T> data, int reportId);
    }
}