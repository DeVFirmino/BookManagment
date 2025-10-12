/*
------------------------------------------------------------
 USER REPORT DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Represents user-related data used for reporting and exporting.
• Provides a simplified view of user information for Excel reports.
• Focuses on identification, contact, and address data.
 

Usage
• Used in ReportController and ReportService for Excel report generation.
• Helps administrators analyze registered users and their activity status.
------------------------------------------------------------
*/

using BooksApi.Enums;
using BooksApi.Models;

namespace BooksApi.Dto.Report
{
    public class UserReportDto
    {
        public int Id{ get; set; }

         public string FullName { get; set; } = string.Empty;

         public string Username { get; set; } = string.Empty;

         public string Email { get; set; } = string.Empty;

         public string IsActive { get; set; }

         public string Profile { get; set; }
         
         public string Street { get; set; } = string.Empty;

          public string City { get; set; } = string.Empty;

          public string Number { get; set; } = string.Empty;

          public string ZipCode { get; set; } = string.Empty;

          public string State { get; set; } = string.Empty;
         
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}