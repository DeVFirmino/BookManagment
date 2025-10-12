/*
------------------------------------------------------------
 ERROR VIEW MODEL
------------------------------------------------------------
Purpose  
• Provides a simple structure for displaying error details in views.  
• Commonly used by ASP.NET Core error handling pages.

Main Properties  
• RequestId → Unique identifier for the current request (used for tracing errors).  
• ShowRequestId → Helper property that returns true if a RequestId exists.

Usage  
• Used by the default error view to show debugging information.  
• Helps to identify and track specific failed requests.
------------------------------------------------------------
*/

namespace BooksApi.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}