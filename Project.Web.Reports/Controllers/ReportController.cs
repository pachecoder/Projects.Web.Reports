using DinkToPdf;
using DinkToPdf.Contracts;
using Project.Web.Reports.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;


namespace Project.Web.Reports.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        private IConverter converter;

        public ReportController(IConverter converter)
        {
            this.converter = converter;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var invoice = new InvoiceViewModel { 

                Master =  new Master { 
                    Id = 12345, 
                    Created = DateTime.Now, 
                    DueDate = DateTime.Now, 
                    Amount = "1000.00", 
                    PaymentMethod = "Wire Transfer", 
                    Company = new Company 
                    { 
                                    Address = "Av Evergreen 123456", 
                                    Name = "My Nice Company", 
                                    State = "Carabobo" 
                    },
                    Client = new Client
                    {
                      CompanyName = "Rakatakataka LLC",
                      Email = "johndoe@rakatakataka.com",
                      Name = "John Doe"
                    }
                },
                
                Details = new List<Detail> { new Detail { Item = "Web Page", Amount = 500  }, 
                    new Detail { Item = "Domain", Amount = 250 }, 
                    new Detail { Item = "Support Contract", Amount = 250 } 
                },

                Total = 1000
            };

            string template = System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(AppContext.BaseDirectory + @"Templates\Invoice.cshtml"));

            string html = Engine.Razor.RunCompile(template, "report", typeof(InvoiceViewModel), invoice);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        FooterSettings = { FontSize = 9, Right = "Page [page] of [toPage]" }
                    }
                }
            };

            byte[] pdf = converter.Convert(doc);

            MemoryStream stream = new MemoryStream(pdf);

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "report-employee.pdf"
            };

            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            return responseMessageResult;
        }

    }
}
