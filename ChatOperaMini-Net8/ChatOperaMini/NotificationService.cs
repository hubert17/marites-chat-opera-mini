namespace ChatOperaMini;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using WebPush;

public class NotificationService
{
    private readonly IConfiguration _configuration;
    private readonly VapidDetails _vapidDetails;

    public NotificationService(IConfiguration configuration)
    {
        _configuration = configuration;

        // Retrieve VAPID keys from configuration
        var publicKey = _configuration["VapidKeys:PublicKey"];
        var privateKey = _configuration["VapidKeys:PrivateKey"];
        var subject = _configuration["VapidKeys:Subject"];

        // Initialize VAPID details
        _vapidDetails = new VapidDetails(subject, publicKey, privateKey);
    }

    public void SendPushNotification(string endpoint, string p256dh, string auth, string payload)
    {
        var webPushSubscription = new WebPush.PushSubscription(endpoint, p256dh, auth);
        var webPushClient = new WebPushClient();

        try
        {
            webPushClient.SendNotification(webPushSubscription, payload, _vapidDetails);
        }
        catch (WebPushException exception)
        {
            Console.WriteLine("Http STATUS code" + exception.StatusCode);
        }
    }
}

public class PushSubscription
{
    public int Id { get; set; }
    public string Sender { get; set; }
    public string ChannelCode { get; set; }
    public string Endpoint { get; set; }
    public string P256DH { get; set; }
    public string Auth { get; set; }
}
public class PushSubscriptionPayload
{
    public string endpoint { get; set; }
    public Keys keys { get; set; }
    public class Keys
    {
        public string p256dh { get; set; }
        public string auth { get; set; }
    }

}

public class PushNotificationPayload
{
    public string title { get; set; }
    public string message { get; set; }
    public string url { get; set; }
    public string icon { get; set; } = @"/images/icons/icon-192x192.png";
    public string click_action { get; set; }
    public string sender { get; set; }
}