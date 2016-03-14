using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.Models;

namespace Snowflake.Core.Domain
{
    public class Choice
    {
        // Primary Key
        public int ChoiceId { get; set; }

        // Foreign Key
        public int ThoughtId { get; set; }
        public int UserId { get; set; }

        // Properties
        public bool Chosen { get; set; }

        // Relationship
        public virtual SnowflakeUser User { get; set; }
        public virtual Thought Thought { get; set; }

        public void Update(ChoiceModel modelChoice)
        {
            ChoiceId = modelChoice.ChoiceId;
            ThoughtId = modelChoice.ThoughtId;
            UserId = modelChoice.UserId;
            Chosen = modelChoice.Chosen;
        }
    }
}
