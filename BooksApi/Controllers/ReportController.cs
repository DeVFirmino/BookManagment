/*
------------------------------------------------------------
 REPORT CONTROLLER
------------------------------------------------------------
Purpose
• Generates Excel reports for books, users (clients/employees),
  and borrows (returned/pending).

Endpoints
• GET /Report/Index
  - Shows the report selection page.

• GET /Report/Generate/{id}
  - id = 1: Books report
  - id = 2: Clients report
  - id = 3: Employees report
  - id = 4: Returned borrows
  - id = 5: Pending borrows
  - Builds a DataTable via ReportService and exports as .xlsx
    using ClosedXML.

Dependencies
• ISessionInterface – session validation (who is logged in)
• IBookInterface – fetch books
• IUserInterface – fetch users (clients/employees)
• IBorrowInterface – fetch borrows (returned/pending)
• IReportInterface – map data to DataTable
• IMapper – map domain models to Report DTOs

Access Control
• Requires authentication ([UserLogged]) and blocks client-only
  access ([UserLoggedClient]) where applicable.

User Feedback
• Uses TempData["ErrorMessage"] when there is no data to export.

Output
• Returns an Excel file (application/vnd.openxmlformats-officedocument.spreadsheetml.sheet)
  named "Data.xls" with a single "Data" worksheet.
------------------------------------------------------------
*/

using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using BooksApi.Dto.Report;
using BooksApi.Filter;
using BooksApi.Models;
using BooksApi.Services.BorrowService;
using BooksApi.Services.BookService;
using BooksApi.Services.ReportService;
using BooksApi.Services.SessionService;
using BooksApi.Services.UserService;
using System.Data;
using BooksApi.Filter;

namespace BooksApi.Controllers
{
   
    [UserLogged]
    [UserLoggedClient]
    public class ReportController : Controller
    {
        private readonly ISessionInterface _sessionInterface;
        private readonly IBookInterface _bookInterface;
        private readonly IUserInterface _userInterface;
        private readonly IBorrowInterface _borrowInterface;
        private readonly IReportInterface _reportInterface;
        private readonly IMapper _mapper;

        public ReportController(
            ISessionInterface sessionInterface,
            IBookInterface bookInterface,
            IUserInterface userInterface,
            IBorrowInterface borrowInterface,
            IReportInterface reportInterface,
            IMapper mapper)
        {
            _sessionInterface = sessionInterface;
            _bookInterface = bookInterface;
            _userInterface = userInterface;
            _borrowInterface = borrowInterface;
            _reportInterface = reportInterface;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Generate(int id)
        {
            var table = new DataTable();

            switch (id)
            {
                case 1:
                    List<BooksModel> books = await _bookInterface.SearchBooks();
                    List<BookReportDto> bookData = _mapper.Map<List<BookReportDto>>(books);

                    if (books.Count == 0)
                    {
                        TempData["ErrorMessage"] = "There is no data available for this report!";
                        return RedirectToAction("Index", "Report");
                    }

                    table = _reportInterface.CollectData(bookData, id);
                    break;

                case 2:
                    List<UserModel> clients = await _userInterface.FindUsers(0);
                    List<UserReportDto> clientData = new List<UserReportDto>();

                    if (clients.Count > 0)
                    {
                        foreach (var client in clients)
                        {
                            clientData.Add(new UserReportDto
                            {
                                Id = client.Id,
                                FullName = client.FullName,
                                Username = client.Username,
                                Email = client.Email,
                                Profile = client.Profile.ToString(),
                                Street = client.Address.Street,
                                Number = client.Address.Number,
                                ZipCode = client.Address.ZipCode,
                                State = client.Address.State,
                                
                            });
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "There is no data available for this report!";
                        return RedirectToAction("Index", "Report");
                    }

                    table = _reportInterface.CollectData(clientData, id);
                    break;

                case 3:
                    List<UserModel> employees = await _userInterface.FindUsers(null);
                    List<UserReportDto> employeeData = new List<UserReportDto>();

                    if (employees.Count > 0)
                    {
                        foreach (var employee in employees)
                        {
                            employeeData.Add(new UserReportDto
                            {
                                Id = employee.Id,
                                FullName = employee.FullName,
                                Username = employee.Username,
                                Email = employee.Email,
                                Profile = employee.Profile.ToString(),
                                Street = employee.Address.Street,
                                Number = employee.Address.Number,
                                ZipCode = employee.Address.ZipCode,
                                State = employee.Address.State,
                            });
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "There is no data available for this report!";
                        return RedirectToAction("Index", "Report");
                    }

                    table = _reportInterface.CollectData(employeeData, id);
                    break;

                case 4:
                    List<BorrowModel> returnedBorrows = await _borrowInterface.SearchAllBorrows(null);
                    List<BorrowReportDto> borrowData = new List<BorrowReportDto>();

                    if (returnedBorrows.Count > 0)
                    {
                        foreach (var borrow in returnedBorrows)
                        {
                            borrowData.Add(new BorrowReportDto
                            {
                                Id = borrow.Id,
                                UserId = borrow.UserId,
                                FullName = borrow.User.FullName,
                                Username = borrow.User.Username,
                                BookId = borrow.BookId,
                                Title = borrow.Book.Title,
                                BorrowDate = borrow.BorrowDate,
                                ReturnDate = borrow.ReturnDate ?? DateTime.MinValue
                            });
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "There is no data available for this report!";
                        return RedirectToAction("Index", "Report");
                    }

                    table = _reportInterface.CollectData(borrowData, id);
                    break;

                case 5:
                    List<BorrowModel> pendingBorrows = await _borrowInterface.SearchAllBorrows("pending");
                    List<BorrowReportDto> pendingBorrowData = new List<BorrowReportDto>();

                    if (pendingBorrows.Count > 0)
                    {
                        foreach (var borrow in pendingBorrows)
                        {
                            pendingBorrowData.Add(new BorrowReportDto
                            {
                                Id = borrow.Id,
                                UserId = borrow.UserId,
                                FullName = borrow.User.FullName,
                                Username = borrow.User.Username,
                                BookId = borrow.BookId,
                                Title = borrow.Book.Title,
                                BorrowDate = borrow.BorrowDate,
                                // ReturnDate = borrow.ReturnDate ?? DateTime.MinValue //check later
                            });
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "There is no data available for this report!";
                        return RedirectToAction("Index", "Report");
                    }

                    table = _reportInterface.CollectData(pendingBorrowData, id);
                    break;
            }

            using (XLWorkbook workbook = new())
            {
                workbook.AddWorksheet(table, "Data");

                using (MemoryStream ms = new())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Data.xls");
                } 
            }
        }
    }
}