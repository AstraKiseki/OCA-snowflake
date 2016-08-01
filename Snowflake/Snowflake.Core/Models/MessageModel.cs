using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snowflake.Core.Models
{
    public class MessageModel
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

        public SnowflakeUserModel User { get; set; }

        public string username
        {
            get
            {
                return User?.UserName;
            }
        }
        public string content
        {
            get
            {
                return Text;
            }
        }
    }
}
