using dashboard_host;

var listener = new UdpListener();
Console.WriteLine("Listening...");
listener.StartReceiving();
