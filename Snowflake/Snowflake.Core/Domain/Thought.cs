using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.Models;

namespace Snowflake.Core.Domain
{
    public enum Sentiment
    {   
        Neutral = 0,
        Positive = 1,
        Negative = 2,
        None = 3
    }

    public class Thought
    {
        // Primary Key
        public int ThoughtId { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Properties
        public string Text { get; set; } // How do I control the string length?
        public int? NumberOfFlags { get; set; }
        public Sentiment Sentiment { get; set; }
        public string Language { get; set; }

        // Relationships
        public virtual ICollection<Choice> Choices { get; set; }
        public virtual SnowflakeUser User { get; set; }

        public void Update(ThoughtModel modelThought)
        {
            ThoughtId = modelThought.ThoughtId;
            UserId = modelThought.UserId;
            Text = modelThought.Text;
            NumberOfFlags = modelThought.NumberOfFlags;
            Sentiment Sentiment = modelThought.Sentiment;
            Language = modelThought.Language;
        }
    }
}
