﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Snowflake.Core.Models
{
    public class SnowflakeUserModel
    {
        // Primary Keys
        public int Id { get; set; }

        // Foreign Keys

        // Properties
        public string UserName { get; set; }
        public int? NumberOfFlags { get; set; }
        public bool ConnectionOpened { get; set; }

    }
}
