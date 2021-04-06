using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ToyBlockFactoryWebAppTests
{
    public class HttpRequestTests: IClassFixture<ToyBlockOrdersFixture>
    {
        
        private readonly ToyBlockOrdersFixture _toyBlockOrdersFixture;

        public HttpRequestTests(ToyBlockOrdersFixture toyBlockOrdersFixture)
        {
            _toyBlockOrdersFixture = toyBlockOrdersFixture;
        }


        [Fact]
        public async Task AppIsAbleToBeAccessedViaWeb()       //TODO: "DEPLOYED" -> too technical?
        { 
            var response = await _toyBlockOrdersFixture.Client.GetAsync("http://localhost:3000/health");
            var statusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Contains("ok", responseBody);
        }

        [Fact]
        public async Task CanCreateOrder_AndGetOrderId()
        {
            var request = _toyBlockOrdersFixture.CreateOrderRequest();
            
            var requestBody = await _toyBlockOrdersFixture.Client.PostAsync("http://localhost:3000/order", request);
            var statusCode = requestBody.StatusCode;
            var responseBody = await requestBody.Content.ReadFromJsonAsync<string>();

            Assert.Equal(HttpStatusCode.Accepted, statusCode);
            Assert.NotNull(responseBody);
            Assert.Matches("[0-9]{4}", responseBody);
        }
        
        [Fact]
        public async Task CanAddBlocksToOrder()
        {
            var orderRequest = _toyBlockOrdersFixture.CreateOrderRequest();
            var orderResponse = await _toyBlockOrdersFixture.Client.PostAsync("http://localhost:3000/order", orderRequest);
            var orderNumber = await orderResponse.Content.ReadFromJsonAsync<string>();
            var blockOrderRequest = _toyBlockOrdersFixture.AddBlocks();
            
            var requestBody = await _toyBlockOrdersFixture.Client.PostAsync($"http://localhost:3000/addblock?orderId={orderNumber}", blockOrderRequest);
            var statusCode = requestBody.StatusCode;
            //TODO:SHOULD I BE RETURNING SOMETHING? Or is this enough to show it worked?
                                                
            Assert.Equal(HttpStatusCode.Accepted, statusCode);
        }

        [Fact]
        public async Task CanDeleteOrder()
        {
            var orderRequest = _toyBlockOrdersFixture.CreateOrderRequest();
            var orderResponse = await _toyBlockOrdersFixture.Client.PostAsync("http://localhost:3000/order", orderRequest);
            var orderNumber = await orderResponse.Content.ReadFromJsonAsync<string>();
            var blockOrderRequest = _toyBlockOrdersFixture.AddBlocks();
            var blockOrderRequestBody = await _toyBlockOrdersFixture.Client.PostAsync($"http://localhost:3000/addblock?orderId={orderNumber}", blockOrderRequest);
           
            var deletedOrder = await _toyBlockOrdersFixture.Client.DeleteAsync($"http://localhost:3000/order?orderId={orderNumber}");
            var statusCode = deletedOrder.StatusCode;

            Assert.Equal(HttpStatusCode.Accepted, statusCode);
        }

        [Fact]
        public async Task CanSubmitOrder()
        {
            var orderRequest = _toyBlockOrdersFixture.CreateOrderRequest();
            var orderResponse = await _toyBlockOrdersFixture.Client.PostAsync("http://localhost:3000/order", orderRequest);
            var orderNumber = await  orderResponse.Content.ReadFromJsonAsync<string>();
            var blockOrderRequest = _toyBlockOrdersFixture.AddBlocks();
            var body = await _toyBlockOrdersFixture.Client.PostAsync($"http://localhost:3000/addblock?orderId={orderNumber}", blockOrderRequest);

            var putContent = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var submittedOrder = await _toyBlockOrdersFixture.Client.PutAsync($"http://localhost:3000/order?orderid={orderNumber}", putContent);
            var statusCode = submittedOrder.StatusCode;
            
            Assert.Equal(HttpStatusCode.Accepted, statusCode);
        }

        [Fact]
        public async Task CanGetReport()
        {
            var orderRequest = _toyBlockOrdersFixture.CreateOrderRequest();
            var orderResponse = await _toyBlockOrdersFixture.Client.PostAsync("http://localhost:3000/order", orderRequest);
            var orderNumber = await orderResponse.Content.ReadFromJsonAsync<string>();
            var blockOrderRequest = _toyBlockOrdersFixture.AddBlocks();
            var body = _toyBlockOrdersFixture.Client.PostAsync($"http://localhost:3000/addblock?orderId={orderNumber}", blockOrderRequest);
        
            var order = await _toyBlockOrdersFixture.Client.GetAsync($"http://localhost:3000/report?orderid={orderNumber}&ReportType=Invoice");
            var statusCode = order.StatusCode;
            var responseBody = await order.Content.ReadAsStringAsync();
        
            Assert.Equal(HttpStatusCode.Accepted, statusCode);
            Assert.Equal(await File.ReadAllTextAsync("InvoiceReport.txt"), responseBody);
        }
        
        
    }
}
