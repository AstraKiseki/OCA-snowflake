using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.Models;

namespace Snowflake.Core.Domain
{
   public class Conversation
    {
        // Primary Key
        public int ConversationId { get; set; }

        // Foreign Keys

        // Properties

        // Relationships
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }

        public void Update(ConversationModel modelConversation)
        {
            ConversationId = modelConversation.ConversationId;
        }
    }
}
