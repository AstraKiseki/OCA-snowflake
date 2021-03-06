﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snowflake.Core.Models
{
    public class ConversationModel
    {
        public int ConversationId { get; set; }
        public IEnumerable<MessageModel> Messages { get; set; }
        public IEnumerable<SnowflakeUserModel> Users { get; set; }
    }
}
