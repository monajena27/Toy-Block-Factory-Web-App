using System;
using System.Collections.Generic;

namespace ToyBlockFactoryKata
{
    public class ToyBlockFactory
    {
        private readonly OrderManagementSystem _orderManagementSystem = new();
        private readonly ReportGenerator _reportGenerator;

        public ToyBlockFactory() : this(new PricingCalculator()) //understand how this works
        {
        }

        public ToyBlockFactory(IInvoiceCalculationStrategy priceCalculator)
        {
            _reportGenerator = new ReportGenerator(priceCalculator);
        }

        public Order CreateOrder(string customerName, string customerAddress)
        {
            return new(customerName, customerAddress);
        }

        public Order CreateOrder(string customerName, string customerAddress, DateTime dueDate)
        {
            return new(customerName, customerAddress, dueDate);
        }

        public string SubmitOrder(Order customerOrder)
        {
            return _orderManagementSystem.SubmitOrder(customerOrder);
        }

        public Order GetOrder(string orderId)
        {
            var orderExists = _orderManagementSystem.GetOrder(orderId, out var order);
            if (!orderExists)
                throw new ArgumentException("This order does not exist!");
            return order;
        }

        public bool OrderExists(string orderId) //is this just for console? so then should test also use this?
        {
            return _orderManagementSystem.GetOrder(orderId, out _);
        }

        public IReport GetInvoiceReport(string orderId)
        {
            var requestedOrder = GetOrder(orderId);
            return _reportGenerator.GenerateInvoice(requestedOrder);
        }

        public IReport GetCuttingListReport(string orderId)
        {
            var requestedOrder = GetOrder(orderId);
            return _reportGenerator.GenerateCuttingList(requestedOrder);
        }

        public IReport GetPaintingReport(string orderId)
        {
            var requestedOrder = GetOrder(orderId);
            return _reportGenerator.GeneratePaintingReport(requestedOrder);
        }

        public List<IReport> GetCuttingListsByDate(DateTime date)
        {
            var orderRecords = _orderManagementSystem.OrderRecords;
            return _reportGenerator.FilterCuttingReportsByDate(date, orderRecords);
        }

        public List<IReport> GetPaintingReportsByDate(DateTime date)
        {
            var orderRecords = _orderManagementSystem.OrderRecords;
            return _reportGenerator.FilterPaintingReportsByDate(date, orderRecords);
        }
    }
}