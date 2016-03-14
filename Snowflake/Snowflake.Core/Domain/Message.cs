using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.Models;

namespace Snowflake.Core.Domain
{
    public class Message
    {
        // Primary Key
        public int MessageId { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int ConversationId { get; set; }

        // Properties
        public int MessageOrder { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }

        // Relationships
        public virtual SnowflakeUser User { get; set; }
        public virtual Conversation Conversation { get; set; }

        public void Update(MessageModel modelMessage)
        {
            MessageId = modelMessage.MessageId;
            UserId = modelMessage.UserId;
            ConversationId = modelMessage.ConversationId;
            MessageOrder = modelMessage.MessageOrder;
            Timestamp = modelMessage.Timestamp;
            Text = modelMessage.Text;
        }
    }
}
