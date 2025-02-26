using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PruebatecnicaBack.Application.Common.Interfaces.scrapping;
using PruebatecnicaBack.Application.Common.Responses;
using PruebatecnicaBack.Domain.Entities;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace PruebatecnicaBack.Infrastructure.Services.Scraper;

public class ScraperService : IScraperService
{
    public async Task<List<Zone>> GetData(int year)
    {
        var products = new List<ProductResult>();

        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        using var driver = new ChromeDriver(options);
        List<string> downloadedFiles = new List<string>();

        try
        {
            driver.Navigate().GoToUrl("https://www.cenace.gob.mx/Paginas/SIM/CapacidadDemandadaRAP.aspx");
            Thread.Sleep(500);

            var yearDropdown = new SelectElement(driver.FindElement(By.Id("ContentPlaceHolder1_DrpAnio")));
            yearDropdown.SelectByValue(year.ToString());
            Thread.Sleep(500);

            var xlsLinks = driver.FindElements(By.XPath("//a[contains(@href, '.xls')]"));
            List<string> urls = new List<string>();

            foreach (var link in xlsLinks)
            {
                string url = link.GetAttribute("href");
                if (!string.IsNullOrEmpty(url))
                {
                    urls.Add(url);
                }
            }

            Console.WriteLine($"Se encontraron {urls.Count} archivos XLS.");

            string downloadPath = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
            if (!Directory.Exists(downloadPath))
                Directory.CreateDirectory(downloadPath);

            using var client = new WebClient();
            int count = 1;

            foreach (string url in urls)
            {
                string filePath = Path.Combine(downloadPath, $"Reporte_{year}_{count}.xls");
                client.DownloadFile(url, filePath);
                downloadedFiles.Add(filePath);
                Console.WriteLine($"Archivo descargado: {filePath}");
                count++;
            }

            List<Zone> allData = new List<Zone>();

            foreach (var file in downloadedFiles)
            {
                var jsonData = ReadExcelToJson(file, year);
                allData.AddRange(jsonData);
            }

            return allData;
        }
        finally
        {
            driver.Quit();
        }
    }

    private List<Zone> ReadExcelToJson(string filePath, int year)
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[0];

        int rows = worksheet.Dimension.Rows;
        int cols = worksheet.Dimension.Columns;

        List<Zone> collection = new List<Zone>();

        string zoneName = worksheet.Cells[9, 1].Text;

        for (int row = 9; row <= rows; row++)
        {
            string participant = worksheet.Cells[row, 2].Text;

            var zone = new Zone
            {
                Name = zoneName,
                Anio = year,
                Participant = participant,
                SubAccount = worksheet.Cells[row, 3].Text,
                CapacidadDemandada = decimal.Parse(worksheet.Cells[row, 4].Text),
                RequisitoAnualDePotencia = decimal.Parse(worksheet.Cells[row, 5].Text),
                ValorDelRequisitoAnualEficiente = decimal.Parse(worksheet.Cells[row, 6].Text)
            };

            collection.Add(zone);
        }

        return collection;
    }
}
