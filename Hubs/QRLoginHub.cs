using Microsoft.AspNetCore.SignalR;

namespace MultiDeviceQrLogin.Hubs;

public sealed class QRLoginHub : Hub
{

    private static Dictionary<string, string> uuidTokenPairs = new Dictionary<string, string>();

    public override async Task OnConnectedAsync()
    {
        // Generate a QR code and send it to the connected client immediately upon connection
        var token = GenerateToken(); // Implement your token generation logic
        await Clients.Caller.SendAsync("ReceiveQRCode", token);

        await base.OnConnectedAsync();
    }
    public async Task GenerateQRCode()
    {
        // Generate the initial token
        string initialToken = GenerateToken();
        await Clients.Caller.SendAsync("ReceiveQRCode", initialToken);

        // Set up a timer to send a new token every 2 minutes
        var timer = new System.Threading.Timer(async _ =>
        {
            string newToken = GenerateToken();
            await Clients.Caller.SendAsync("ReceiveQRCode", newToken);
        }, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
    }

    private string GenerateToken()
    {
        // Generate a unique token (e.g., a GUID)
        string token = Guid.NewGuid().ToString();

        // Store the token in the server-side dictionary with the connection ID as the key
        uuidTokenPairs[Context.ConnectionId] = token;

        return token;
    }

    public async Task VerifyQRCode(string scannedToken)
    {
        // Check if the scanned token matches any stored tokens
        var matchingConnectionId = uuidTokenPairs.FirstOrDefault(pair => pair.Value == scannedToken).Key;

        if (!string.IsNullOrEmpty(matchingConnectionId))
        {
            // Successful login, send a message to User 'A' (the generator of the QR code)
            await Clients.Client(matchingConnectionId).SendAsync("LoginSuccess", "Successfully Logged In!");

            // Disconnect User 'A's WebSocket connection
            await Clients.Client(matchingConnectionId).SendAsync("Disconnect");

            // Remove the pair from the dictionary
            uuidTokenPairs.Remove(matchingConnectionId);
        }
        else
        {
            // UUID not found, send an error message to User 'B'
            await Clients.Caller.SendAsync("LoginError", "UUID not found or does not match!");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Remove the disconnected client from the dictionary
        if (uuidTokenPairs.ContainsKey(Context.ConnectionId))
        {
            uuidTokenPairs.Remove(Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}