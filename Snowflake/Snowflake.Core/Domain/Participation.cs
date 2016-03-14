using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.Models;

namespace Snowflake.Core.Domain
{
    public class Participation
    {
        // Composite Key
        public int UserId { get; set; }
        public int ConversationId { get; set; }

        // Relationships
        public virtual SnowflakeUser User { get; set; }
        public virtual Conversation Conversation { get; set; }

        public void Update(ParticipationModel modelParticipation)
        {
            UserId = modelParticipation.UserId;
            ConversationId = modelParticipation.ConversationId;
        }
    }
}
