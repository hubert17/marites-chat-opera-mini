using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatOperaMini.Models;

public class Message
{
    public int Id { get; set; }

    [StringLength(255)]
    public string ChannelCode { get; set; }

    [StringLength(255)]
    public string Sender { get; set; }

    public string MessageText { get; set; }

    public DateTime SendDate { get; set; }

    [NotMapped]
    public bool IsRead { get { return MessageReads.Any(a => a.MessageId == Id); } }

    public virtual List<MessageRead> MessageReads { get; set; } = new List<MessageRead>();


    public static List<Message> SeedData()
    {
        return new List<Message>
    {
        new Message
        {
            Id = 1,
            ChannelCode = "public",
            Sender = "Mama",
            MessageText = "Hi Zoey. I'll see you later.",
            SendDate = DateTime.Now.AddMinutes(-4),
        },
        new Message
        {
            Id = 2,
            ChannelCode = "public",
            Sender = "Zoey",
            MessageText = "Hi mama, our class is about to finish.",
            SendDate = DateTime.Now.AddMinutes(-3),
        },
        new Message
        {
            Id = 3,
            ChannelCode = "public",
            Sender = "Papa",
            MessageText = "I am driving home.",
            SendDate = DateTime.Now.AddMinutes(-2),
        },
        new Message
        {
            Id = 4,
            ChannelCode = "public",
            Sender = "Mama",
            MessageText = "Zoey, are you there?",
            SendDate = DateTime.Now
        }
    };
    }

}
