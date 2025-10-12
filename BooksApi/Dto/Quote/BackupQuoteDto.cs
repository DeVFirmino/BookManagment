/*
------------------------------------------------------------
 QUOTE DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Represents a quote fetched from an external public API.
• Used to display inspirational quotes on the homepage.
• Acts as a BACKUP in case the API Fails

Fields
• content – The text of the quote itself.
• author  – The person who said or wrote the quote.

Usage
• Used in HomeController to consume data from the Quotable or DummyJSON API.
• The quote is displayed dynamically in the Home/Index view.
------------------------------------------------------------
*/

namespace BooksApi.Dto.Quote
{
    public class BackupQuoteDto
    {
        public string quote { get; set; }
        public string author { get; set; }
    }
}