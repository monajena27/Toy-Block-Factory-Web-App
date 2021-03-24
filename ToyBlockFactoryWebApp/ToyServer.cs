using System;
using System.Net;
using ToyBlockFactoryKata;

namespace ToyBlockFactoryWebApp
{
    class ToyServer
    {
        private readonly HttpListener _httpListener;
        private readonly string[] _uri;
        public readonly HealthCheckController _healthCheckController;
        private readonly Router _router;
        private OrderController _orderController;

        public ToyServer(string[] prefixes, ToyBlockFactory toyBlockFactory)
        {
            if (prefixes == null || prefixes.Length == 0)
            {
                throw new ArgumentException("prefixes");
            }
            _uri = prefixes;
            _httpListener = new HttpListener();
            _healthCheckController = new HealthCheckController();
            _orderController = new OrderController(toyBlockFactory);
            _router = new Router(_healthCheckController, _orderController);
            Start();
        }

        private void Start()
        {
            foreach (string s in _uri)
            {
                _httpListener.Prefixes.Add(s);
            }
            _httpListener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext(); 
                try
                {
                    //var request = context.Request;
                    Console.WriteLine($"\nReceived request from: {context.Request.Url.PathAndQuery}");
                    _router.ReadRequests(context);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    context.Response.StatusCode = 500;
                    context.Response.Close();
                }
            }

            _httpListener.Stop();
        }
    }
    
}