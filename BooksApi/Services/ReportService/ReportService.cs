/*
------------------------------------------------------------
 REPORT SERVICE (ReportService)
------------------------------------------------------------
Purpose
• Builds DataTables for Excel export based on a selected report type.
• Centralizes “report shaping” logic so controllers stay thin.

How it works
• CollectData<T>(data, reportId)
  - Creates a DataTable named after ReportEnum.
  - Adds columns from the DTO properties (handles nullable types).
  - Routes to the proper exporter by reportId:
      1 → ExportBooks            (BookReportDto)
      2 → ExportUsers            (UserReportDto - Clients)
      3 → ExportUsers            (UserReportDto - Employees)
      4 → ExportReturnedBorrows  (BorrowReportDto)
      5 → ExportPendingBorrows   (BorrowReportDto)

Export methods
• ExportBooks(DataTable, List<BookReportDto>)
• ExportUsers(DataTable, List<UserReportDto>)
• ExportReturnedBorrows(DataTable, List<BorrowReportDto>)
• ExportPendingBorrows(DataTable, List<BorrowReportDto>)
  - Each method fills rows in the expected column order for Excel.

Used by
• ReportController.Generate → receives a ready-to-export DataTable
  which is then written to an .xlsx file with ClosedXML.
------------------------------------------------------------
*/

using AutoMapper;
using Newtonsoft.Json;
using BooksApi.Dto.Report;
using BooksApi.Enums;
using System.Data;

namespace BooksApi.Services.ReportService
{
    public class ReportService : IReportInterface
    {
        private readonly IMapper _mapper;

        public ReportService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public DataTable CollectData<T>(List<T> data, int reportId)
        {
            DataTable dataTable = new DataTable();

            dataTable.TableName = Enum.GetName(typeof(ReportEnum), reportId);

            var columns = data.First().GetType().GetProperties();

            foreach (var column in columns)
            {
                var columnType = Nullable.GetUnderlyingType(column.PropertyType) ?? column.PropertyType;
                dataTable.Columns.Add(column.Name, columnType);
            }

            switch (reportId)
            {
                case 1:
                    var bookData = JsonConvert.SerializeObject(data);
                    var bookDataModel = JsonConvert.DeserializeObject<List<BookReportDto>>(bookData);
                    if (bookDataModel != null)
                    {
                        return ExportBooks(dataTable, bookDataModel);
                    }
                    break;

                case 2:
                    var clientData = JsonConvert.SerializeObject(data);
                    var clientDataModel = JsonConvert.DeserializeObject<List<UserReportDto>>(clientData);
                    if (clientDataModel != null)
                    {
                        return ExportUsers(dataTable, clientDataModel);
                    }
                    break;

                case 3:
                    var employeeData = JsonConvert.SerializeObject(data);
                    var employeeDataModel = JsonConvert.DeserializeObject<List<UserReportDto>>(employeeData);
                    if (employeeDataModel != null)
                    {
                        return ExportUsers(dataTable, employeeDataModel);
                    }
                    break;

                case 4:
                    var borrowData = JsonConvert.SerializeObject(data);
                    var borrowDataModel = JsonConvert.DeserializeObject<List<BorrowReportDto>>(borrowData);
                    if (borrowDataModel != null)
                    {
                        return ExportReturnedBorrows(dataTable, borrowDataModel);
                    }
                    break;

                case 5:
                    var pendingBorrowData = JsonConvert.SerializeObject(data);
                    var pendingBorrowDataModel = JsonConvert.DeserializeObject<List<BorrowReportDto>>(pendingBorrowData);
                    if (pendingBorrowDataModel != null)
                    {
                        return ExportPendingBorrows(dataTable, pendingBorrowDataModel);
                    }
                    break;
            }

            return new DataTable();
        }

        public DataTable ExportBooks(DataTable table, List<BookReportDto> data)
        {
            foreach (var item in data)
            {
                table.Rows.Add(
                    item.Id,
                    item.UserId,
                    item.FullName,
                    item.User,
                    item.BookId,
                    item.Title,
                    item.BorrowDate,
                    item.ReturnDate
                );
            }

            return table;
        }

        public DataTable ExportUsers(DataTable table, List<UserReportDto> data)
        {
            foreach (var item in data)
            {
                table.Rows.Add(
                    item.Id,
                    item.FullName,
                    item.Username,
                    item.Email,
                    item.IsActive == "True" ? "Active" : "Inactive",
                    item.Profile,
                    item.Street,
                    item.City,
                    item.Number,
                    item.ZipCode,
                    item.State,
                    item.CreatedAt,
                    item.UpdatedAt
                );
            }

            return table;
        }

        public DataTable ExportReturnedBorrows(DataTable table, List<BorrowReportDto> data)
        {
            foreach (var item in data)
            {
                table.Rows.Add(item.Id, item.UserId, item.FullName, item.Username,
                               item.BookId, item.Title, item.BorrowDate, item.ReturnDate);
            }

            return table;
        }

        public DataTable ExportPendingBorrows(DataTable table, List<BorrowReportDto> data)
        {
            foreach (var item in data)
            {
                table.Rows.Add(item.Id, item.UserId, item.FullName, item.Username,
                               item.BookId, item.Title, item.BorrowDate);
            }

            return table;
        }
    }
}