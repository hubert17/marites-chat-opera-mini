namespace ChatOperaMini.Models;

public class MessageViewModel
{
    public string ChannelCode { get; set; }
    public string Sender { get; set; }    
    public Message NewMessage { get; set; }
    public List<Message> UnreadMessages { get; set; }
    public List<Message> Messages { get; set; }
    public int PageNo { get; set; }
    public int PageTotal { get; set; }
}
