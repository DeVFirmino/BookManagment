/*
------------------------------------------------------------
 BOOK REPORT DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Represents the structure of book lending data used in reports.
• Provides essential information about each borrowing record.
• Used to export data to Excel files through the reporting system.

Fields
• Id          – Unique identifier of the borrowing record.
• UserId      – Identifier of the user who borrowed the book.
• FullName    – Full name of the user.
• User        – Username or login of the borrower.
• BookId      – Identifier of the borrowed book.
• Title       – Title of the borrowed book.
• BorrowDate  – Date and time when the book was borrowed.
• ReturnDate  – Date and time when the book was returned (nullable).

Usage
• Used by ReportController and ReportService to generate Excel reports.
• Facilitates tracking of borrowing history and user activity.
------------------------------------------------------------
*/

using BooksApi.Models;

namespace BooksApi.Dto.Report
{
    public class BookReportDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; }

        public string User { get; set; }

        public int BookId { get; set; }

        public string Title { get; set; }

        public DateTime BorrowDate { get; set; } = DateTime.Now;

        public DateTime? ReturnDate { get; set; }
    }
}