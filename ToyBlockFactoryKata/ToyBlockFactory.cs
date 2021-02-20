using System;
using System.Collections.Generic;
using ToyBlockFactoryKata.Orders;
using ToyBlockFactoryKata.PricingStrategy;
using ToyBlockFactoryKata.Reports;
using ToyBlockFactoryKata.ReportSystem;

namespace ToyBlockFactoryKata
{
    public class ToyBlockFactory
    {
        private readonly OrderManagementSystem _orderManagementSystem = new();
        private readonly ReportCreator _reportCreator;

        public ToyBlockFactory(IInvoiceCalculator priceCalculator)
        {
            _reportCreator = new ReportCreator(priceCalculator);
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
            if (customerOrder.BlockList.Count > 0)
            {
                return _orderManagementSystem.SubmitOrder(customerOrder);
            }

            return string.Empty;  
        }

        public bool OrderExists(string orderId) 
        {
            return _orderManagementSystem.GetOrder(orderId, out _);
        }
        
        public Order GetOrder(string orderId)
        {
            var orderExists = _orderManagementSystem.GetOrder(orderId, out var order);
            if (!orderExists)
                throw new ArgumentException("This order does not exist!");
            return order;
        }

        public IReport GetReport(string orderId, ReportType reportType)
        {
            var requestedOrder = GetOrder(orderId);
            return _reportCreator.GenerateReport(requestedOrder, reportType);
        }
        
        
        public IEnumerable<IReport> GetReportsByDate(DateTime date, ReportType reportType)
        {
            var orderRecords = _orderManagementSystem.orderRecords;
            return _reportCreator.FilterReportsByDate(date, orderRecords, reportType);
        }
        
    }
}