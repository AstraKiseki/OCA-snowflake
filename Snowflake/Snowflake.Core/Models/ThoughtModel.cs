using Snowflake.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snowflake.Core.Models
{
    public class ThoughtModel
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

            public SnowflakeUserModel User { get; set; }
    }
}