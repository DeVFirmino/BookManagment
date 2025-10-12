/*
------------------------------------------------------------
 QUOTE DTO (Data Transfer Object)
------------------------------------------------------------
Purpose
• Represents a quote fetched from an external public API.
• Used to display inspirational quotes on the homepage.
• Acts as a simple model for mapping JSON API responses.

Fields
• content – The text of the quote itself.
• author  – The person who said or wrote the quote.

Usage
• Used in HomeController to consume data from the Quotable or DummyJSON API.
• The quote is displayed dynamically in the Home/Index view.
------------------------------------------------------------
*/

namespace BooksApi.Dto.Quote;

public class QuoteDto
{
    public string content { get; set; }
    public string author { get; set; }

}