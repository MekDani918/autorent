using autorent.Models;
using autorent.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace autorent.Services
{
    public class WebsocketDataUpdateService : IDisposable
    {
        private readonly string _wsUrl = new Settings().API_URL?.Replace("http://", "ws://").Replace("https://", "wss://") ?? "ws://127.0.0.1:3000";
        private const int _receiveBufferSize = 8192;

        public delegate void EntryDeletedDelegate(int idOfDeletedItem);
        public delegate void CategoryCreatedDelegate(Category category);
        public delegate void CarCreatedDelegate(Car car);
        public delegate void SaleCreatedDelegate(Sale sale);

        public event CategoryCreatedDelegate OnCategoryCreated;
        public event EntryDeletedDelegate OnCategoryDeleted;
        public event CarCreatedDelegate OnCarCreated;
        public event CarCreatedDelegate OnCarEdited;
        public event EntryDeletedDelegate OnCarDeleted;
        public event SaleCreatedDelegate OnSaleCreated;
        public event EntryDeletedDelegate OnSaleDeleted;
        public event Action OnConnectionClosed;

        private ClientWebSocket _wsClient;
        private CancellationTokenSource _cToken;

        public WebsocketDataUpdateService()
        {
            tryToConnect();
            OnConnectionClosed += tryToConnect;
        }

        public async Task ConnectAsync()
        {
            if (_wsClient != null)
            {
                if (_wsClient.State == WebSocketState.Open) return;
                else _wsClient.Dispose();
            }
            _wsClient = new ClientWebSocket();
            if (_cToken != null) _cToken.Dispose();
            _cToken = new CancellationTokenSource();
            await _wsClient.ConnectAsync(new Uri(_wsUrl), _cToken.Token);
            await Task.Factory.StartNew(receiveLoop, _cToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        public async Task DisconnectAsync()
        {
            if (_wsClient is null) return;
            if (_wsClient.State == WebSocketState.Open)
            {
                _cToken.CancelAfter(TimeSpan.FromSeconds(2));
                await _wsClient.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                await _wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            _wsClient.Dispose();
            _wsClient = null;
            _cToken.Dispose();
            _cToken = null;
        }
        private async Task receiveLoop()
        {
            var loopToken = _cToken.Token;
            MemoryStream outputStream = null;
            WebSocketReceiveResult receiveResult = null;
            var buffer = new byte[_receiveBufferSize];
            try
            {
                while (!loopToken.IsCancellationRequested)
                {
                    outputStream = new MemoryStream(_receiveBufferSize);
                    do
                    {
                        receiveResult = await _wsClient.ReceiveAsync(buffer, _cToken.Token);
                        if (receiveResult.MessageType != WebSocketMessageType.Close)
                            outputStream.Write(buffer, 0, receiveResult.Count);
                    }
                    while (!receiveResult.EndOfMessage);
                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                        break;
                    outputStream.Position = 0;
                    responseReceived(outputStream);
                }
            }
            catch (TaskCanceledException) { }
            catch (WebSocketException e)
            {
                if(e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                {
                    OnConnectionClosed?.Invoke();
                }
            }
            finally
            {
                outputStream?.Dispose();
            }
        }
        private void responseReceived(Stream inputStream)
        {
            string dataString = "";
            using (StreamReader sr  = new StreamReader(inputStream))
            {
                while (!sr.EndOfStream)
                {
                    dataString += sr.ReadLine();
                }
            }
            Debug.WriteLine(dataString);

            WebsocketMessage message = null;
            try
            {
                message = JsonSerializer.Deserialize<WebsocketMessage>(dataString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch {
                inputStream.Dispose();
                return;
            }
            
            switch (message.Action)
            {
                case "carcreated":
                    try
                    {
                        Car car = JsonSerializer.Deserialize<Car>(message.Data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        OnCarCreated?.Invoke(car);
                    }
                    catch { }
                    break;
                case "caredited":
                    try
                    {
                        Car car = JsonSerializer.Deserialize<Car>(message.Data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        OnCarEdited?.Invoke(car);
                    }
                    catch { }
                    break;
                case "cardeleted":
                    try
                    {
                        int carId = int.Parse(message.Data.ToString());
                        OnCarDeleted?.Invoke(carId);
                    }
                    catch { }
                    break;
                case "newcategory":
                    try
                    {
                        Category category = JsonSerializer.Deserialize<Category>(message.Data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        OnCategoryCreated?.Invoke(category);
                    }
                    catch { }
                    break;
                case "categorydeleted":
                    try
                    {
                        int categoryId = int.Parse(message.Data.ToString());
                        OnCategoryDeleted?.Invoke(categoryId);
                    }
                    catch { }
                    break;
                case "newsale":
                    try
                    {
                        Sale sale = JsonSerializer.Deserialize<Sale>(message.Data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        OnSaleCreated?.Invoke(sale);
                    }
                    catch { }
                    break;
                case "saleended":
                    try
                    {
                        int carId = int.Parse(message.Data.ToString());
                        OnSaleDeleted?.Invoke(carId);
                    }
                    catch { }
                    break;
            }

            inputStream.Dispose();
        }

        private async void tryToConnect()
        {
            int count = 10;
            while ((_wsClient == null || _wsClient.State == WebSocketState.Aborted || _wsClient.State == WebSocketState.Closed) && count > 0)
            {
                Debug.WriteLine("");
                Debug.WriteLine($"WS Connection attempts left: {count}");
                try
                {
                    await ConnectAsync();
                }
                catch { }
                count--;
                Thread.Sleep(2000);
            }
        }


        public void Dispose() => DisconnectAsync().Wait();
    }
}
