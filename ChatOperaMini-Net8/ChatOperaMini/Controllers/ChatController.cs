using ChatOperaMini.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text.Json;
using WebPush;

namespace ChatOperaMini.Controllers;

public class ChatController : Controller
{
    private readonly AppDbContext _db;
    private readonly IHubContext<ChatHub> _hub;
    private readonly NotificationService _notificationService;
    private readonly IConfiguration _configuration;

    public ChatController(AppDbContext dbContext, IHubContext<ChatHub> hubContext, NotificationService notificationService, IConfiguration configuration)
    {
        _db = dbContext;
        _hub = hubContext;
        _notificationService = notificationService;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index(string sender, string channelCode = "public", string operaMini = "0", int page = 1, int messageId = 0)
    {
        if (string.IsNullOrWhiteSpace(sender))
        {
            return RedirectToAction("Join");
        }

        int pageSize = 5;
        var messageQry = _db.Messages.Include(x => x.MessageReads).Where(x => x.ChannelCode == channelCode).AsQueryable();
        var messages = await messageQry.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        int totalMessages = await messageQry.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalMessages / pageSize);

        var messageVM = new MessageViewModel
        {
            Sender = sender,
            ChannelCode = channelCode,
            UnreadMessages = messages.Where(x => !x.IsRead && !x.Sender.Equals(sender, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.SendDate).ToList(),
            Messages = messages.Where(x => x.IsRead || x.Sender.Equals(sender, StringComparison.OrdinalIgnoreCase)).ToList(),
            PageNo = page,
            PageTotal = totalPages
        };

        ViewBag.MessageId = messageId;
        ViewBag.OperaMini = operaMini != "0" || IsOperaMiniBrowser() ? "1" : "0";
        ViewBag.VapidPublicKey = _configuration["VapidKeys:PublicKey"];

        return View(messageVM);
    }

    [HttpPost]
    public async Task<IActionResult> Index(Message message, int[] unreadMessageIds, string operaMini)
    {
        if(!string.IsNullOrWhiteSpace(message.MessageText))
        {
            message.SendDate = DateTime.Now;
            _db.Messages.Add(message);

            if (unreadMessageIds != null)
            {
                var unreadMessages = await _db.Messages.Where(x => unreadMessageIds.Contains(x.Id)).ToListAsync();
                unreadMessages.ForEach(x =>
                {
                    var messageRead = new MessageRead
                    {
                        MessageId = x.Id,
                        Readby = message.Sender
                    };
                    _db.MessageReads.Add(messageRead);
                });
            }

            await _db.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ReceiveMessage", message.Sender, message.MessageText, message.Id, message.SendDate.ToManilaTime().ToString("M-d-yy h:mmtt"));

            // Trigger Push Notification
            var subscriptions = _db.PushSubscriptions.ToList();

            var payload = new PushNotificationPayload
            {
                Title = $"New Message from {message.Sender}",
                Message = message.MessageText,
                Url = $"/#u{message.Id}"
            };

            var payloadJson = JsonSerializer.Serialize(payload);

            foreach (var subscription in subscriptions)
            {
                _notificationService.SendPushNotification(subscription.Endpoint, subscription.P256DH, subscription.Auth, payloadJson);
            }
        }

        return RedirectToAction("Index", new { sender = message.Sender, channelCode = message.ChannelCode, messageId = message.Id, operaMini });
    }

    public IActionResult Join()
    {
        ViewBag.Sender = HttpContext.Request.Cookies["Sender"];
        ViewBag.ChannelCode = HttpContext.Request.Cookies["ChannelCode"];

        ViewBag.OperaMini = IsOperaMiniBrowser() ? "1" : "0";
        ViewBag.VapidPublicKey = _configuration["VapidKeys:PublicKey"];

        return View();
    }

    [HttpPost]
    public IActionResult Join(string sender, string channelCode = "public", string operaMini = "0")
    {
        var regex = new Regex("^[a-zA-Z0-9@._]*$");
        if ((string.IsNullOrWhiteSpace(sender) || !regex.IsMatch(sender)) || !regex.IsMatch(channelCode))
        {
            TempData["Invalid"] = true;
            return RedirectToAction("Join");
        }

        // Persist sender and channelCode to session cookies (no expiry)
        HttpContext.Response.Cookies.Append("Sender", sender);
        HttpContext.Response.Cookies.Append("ChannelCode", channelCode);

        return RedirectToAction("Index", new { sender, channelCode, operaMini });
    }

    [HttpPost]
    public IActionResult Subscribe([FromBody] PushSubscriptionPayload subscriptionPayload)
    {
        var subscription = new PushSubscription
        {
            Endpoint = subscriptionPayload.endpoint,
            P256DH = subscriptionPayload.keys.p256dh,
            Auth = subscriptionPayload.keys.auth
        };
        _db.PushSubscriptions.Add(subscription);
        _db.SaveChanges();

        return Ok();
    }
    private bool IsOperaMiniBrowser()
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        if (userAgent != null)
        {
            // Check if the User-Agent contains "Opera Mini"
            return userAgent.Contains("Opera Mini", StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }
}
