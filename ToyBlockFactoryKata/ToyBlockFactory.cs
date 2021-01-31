namespace ToyBlockFactoryKata
{
    public class ToyBlockFactory
    {
        private readonly OrderGenerator _orderGenerator = new();
        private readonly ReportGenerator _report = new();
        private Order _customerOrder;

        //IS IT BAD TO INITIALISE HERE OR SHOULD IT BE IN CONSTR?

        public Order CreateOrder(string customerName, string customerAddress)
        {
            _customerOrder = new Order {Name = customerName, Address = customerAddress};
            return _customerOrder;
        }

        public void SubmitOrder(Order customerOrder)
        {
            _orderGenerator.CreateOrder(customerOrder); //.SubmitOrder()???
        }

        public Order GetOrder(string orderId)
        {
            //should we ask customer what report they want?
            var orderExists = _orderGenerator.GetOrder(orderId, out var order);
            return order;
        }

        public Report GetInvoiceReport(string orderId)
        {
            var requestedOrder = GetOrder(orderId);
            return _report.GenerateInvoice(requestedOrder);
        }

        public Report GetCuttingListReport(string orderId)
        {
            var requestedOrder = GetOrder(orderId);
            return _report.GenerateCuttingList(requestedOrder);
        }

        public Report GetPaintingReport(string orderId)
        {
            var requestedOrder = GetOrder(orderId);
            return _report.GeneratePaintingReport(requestedOrder);
        }
    }
}