/*
------------------------------------------------------------
 REPORT ENUM
------------------------------------------------------------
Purpose
• Defines the different types of reports that can be generated
  in the system’s ReportController.

Values
• Books            = 1 → Generates a report of all registered books.
• Clients          = 2 → Generates a report of all clients.
• Employees        = 3 → Generates a report of all employees.
• ReturnedBorrows  = 4 → Generates a report of all returned book borrows.
• PendingBorrows   = 5 → Generates a report of all pending (not yet returned) borrows.

Usage
• Used by ReportController to identify which dataset to fetch and export.
• Improves readability and avoids the use of “magic numbers” in report generation.
------------------------------------------------------------
*/
        
namespace BooksApi.Enums
        {
            public enum ReportEnum
            {
                Books = 1,
                Clients = 2,
                Employees = 3,
                ReturnedBorrows = 4,
                PendingBorrows = 5
            }
        }