using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatOperaMini.Models;

public class MessageRead
{
    public int Id { get; set; }

    [ForeignKey("MessageId")]
    public Message Message { get; set; }
    public int MessageId { get; set; }

    [StringLength(255)]
    public string Readby { get; set; }

    public static List<MessageRead> SeedData()
    {
        return new List<MessageRead>
        {
            new MessageRead { Id = 1, MessageId = 1, Readby = "Zoey"},
            new MessageRead { Id = 2,  MessageId = 2, Readby = "Zoey"},
            new MessageRead { Id = 3,  MessageId = 3, Readby = "Zoey"}
        };
    }
}
