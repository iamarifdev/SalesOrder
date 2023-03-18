using SalesOrder.Data.Models;

namespace SalesOrder.Data.Tests;

public static class TestDataHelper
{
    public static List<SubElement> GetTestSubElements()
    {
        var subElements = new List<SubElement>();
        for (var i = 1; i <= 10; i++)
        {
            subElements.Add(new SubElement
            {
                Id = i,
                Element = i,
                Type = "Window",
                Width = 1000,
                Height = 1000,
                OrderId = i,
                WindowId = i
            });
        }
        return subElements;
    }
}
