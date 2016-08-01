using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snowflake.Core.Models
{
    public class ChoiceModel
    {
        // Primary Key
        public int ChoiceId { get; set; }

        // Foreign Key
        public int ThoughtId { get; set; }
        public int UserId { get; set; }

        // Properties
        public bool Chosen { get; set; }
    }
}
