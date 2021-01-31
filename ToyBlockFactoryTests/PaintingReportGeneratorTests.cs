using System;
using System.Drawing;
using System.Linq;
using ToyBlockFactoryKata;
using Xunit;

namespace ToyBlockFactoryTests
{
    public class PaintingReportGeneratorTests
    {
        private ToyBlockFactory _toyBlockFactory;
        private string _customerName;
        private string _customerAddress;

        public PaintingReportGeneratorTests()
        {
            _toyBlockFactory = new ToyBlockFactory();
            _customerName = "David Rudd";
            _customerAddress = "1 Bob Avenue, Auckland";
            var customerOrder = _toyBlockFactory.CreateOrder(_customerName, _customerAddress);
            customerOrder.AddBlock(Shape.Square, Colour.Red);
            customerOrder.AddBlock(Shape.Square, Colour.Yellow);
            customerOrder.AddBlock(Shape.Triangle, Colour.Blue);
            customerOrder.AddBlock(Shape.Triangle, Colour.Blue);
            customerOrder.AddBlock(Shape.Circle, Colour.Blue);
            customerOrder.AddBlock(Shape.Circle, Colour.Yellow);
            customerOrder.AddBlock(Shape.Circle, Colour.Yellow);
            customerOrder.SetDueDate("19 Jan 2019");
            _toyBlockFactory.SubmitOrder(customerOrder);
        }
        
        [Fact]
        public void IsPaintingReport()
        {
            const string orderId = "0001";

            var paintingReport = _toyBlockFactory.GetPaintingReport(orderId);

            Assert.Equal(ReportType.Painting, paintingReport.ReportType);
        }

        [Fact]
        public void ReportCustomerNameMatchesOrder()
        {
            const string orderId = "0001";

            var paintingReport = _toyBlockFactory.GetPaintingReport(orderId);

            Assert.Equal(_customerName, paintingReport.Name);
        }

        [Fact]
        public void ReportCustomerAddressMatchesOrder()
        {
            const string orderId = "0001";

            var paintingReport = _toyBlockFactory.GetPaintingReport(orderId);

            Assert.Equal(_customerAddress, paintingReport.Address);
        }

        [Fact]
        public void ReportDueDateMatchesOrder()
        {
            const string orderId = "0001";

            var paintingReport = _toyBlockFactory.GetPaintingReport(orderId);

            Assert.Equal(new DateTime(2019, 1, 19), paintingReport.DueDate);
        }

        [Fact]
        public void ReportOrderIdMatchesOrder()
        {
            const string orderId = "0001";

            var paintingReport = _toyBlockFactory.GetPaintingReport(orderId);

            Assert.Equal(orderId, paintingReport.OrderId);
        }
        
        [Theory]
        [InlineData(Shape.Square, "Red", 1)]
        [InlineData(Shape.Square, "Yellow", 1)]
        [InlineData(Shape.Triangle, "Blue", 2)]
        [InlineData(Shape.Circle, "Blue", 1)]
        [InlineData(Shape.Circle, "Yellow", 2)]
        public void ReportShouldGenerateOrderTable(Shape shape, string colour, int quantity)
        {
            const string orderId = "0001";
            var cuttingList = _toyBlockFactory.GetPaintingReport(orderId);

            var tableRow = cuttingList.OrderTable.SingleOrDefault(l => l.Shape == shape);
            var tableColumn = tableRow.TableColumn.SingleOrDefault(l => l.MeasuredItem == colour);

            Assert.NotNull(tableRow);
            Assert.NotNull(tableColumn);      
            Assert.Equal(shape, tableRow.Shape);
            Assert.Equal(colour, tableColumn.MeasuredItem);
            Assert.Equal(quantity, tableColumn.Quantity);
        }
        
    }
}