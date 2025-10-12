/*
------------------------------------------------------------
 BORROW REPORT DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Defines the structure for borrowing data used in report generation.
• Serves as a clean data container for exporting to Excel.
• Helps track user borrow and return activities.

Fields
• Id           – Unique identifier for the borrowing record.
• UserId       – ID of the user who borrowed the book.
• FullName     – Full name of the user.
• Username     – Username of the borrower.
• BookId       – ID of the borrowed book.
• Title        – Title of the borrowed book.
• BorrowDate   – Date when the borrowing occurred.
• ReturnDate   – Date when the book was returned.

Usage
• Used in ReportController and ReportService to generate Excel reports.
• Enables filtering between returned and pending borrows.
------------------------------------------------------------
*/

using BooksApi.Models;

namespace BooksApi.Dto.Report
{
    public class BorrowReportDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; }
    }
}