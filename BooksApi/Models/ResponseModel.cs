/*
------------------------------------------------------------
 RESPONSE MODEL (Generic)
------------------------------------------------------------
Purpose  
• Provides a standardized structure for service and controller responses.  
• Wraps returned data together with a message and a success status flag.

Generic Type  
• T → Represents any data type (e.g., UserModel, BookModel, DTOs).

Main Properties  
• Data → The actual response content (nullable generic type).  
• Message → Informational or error message related to the response.  
• Status → Indicates if the operation was successful (true) or failed (false).

Usage  
• Used across services (e.g., Login, CRUD operations) to maintain consistent responses.  
• Simplifies communication between back-end services and controllers.
------------------------------------------------------------
*/

namespace BooksApi.Models
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}