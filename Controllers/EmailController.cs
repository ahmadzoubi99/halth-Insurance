using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

public class EmailController : Controller
{
    private readonly EmailService _emailService;

    public EmailController(EmailService emailService)
    {
        _emailService = emailService;
    }

    public IActionResult SendEmail()
    {
        try
        {
            // Create a PDF with the message content in-memory
            var message = "Hello, this is a test email!";
            var pdfBytes = CreatePdfWithMessage(message);

            // Send the email with the PDF attachment
            _emailService.SendEmailWithAttachment("ahmadalzoubi01999@gmail.com", "Test Subject", message, pdfBytes);

            return View();
        }
        catch (Exception ex)
        {
            // Handle any exceptions, e.g., log the error
            ViewBag.ErrorMessage = "Error: " + ex.Message;
            return View();
        }
    }

    private byte[] CreatePdfWithMessage(string message)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            // Create a PDF document
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            document.Open();
            document.Add(new Paragraph(message));
            document.Close();

            return ms.ToArray();
        }
    }
}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;

//public class EmailController : Controller
//{
//    private readonly EmailService _emailService;

//    public EmailController(EmailService emailService)
//    {
//        _emailService = emailService;
//    }

//    public IActionResult SendEmail()
//    {
//        _emailService.SendEmail("ahmadalzoubi01999@gmail.com", "Test Subject", "Hello, this is a test email!");
//        return View();
//    }
//}
