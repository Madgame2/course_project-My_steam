﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class SendReportDTO
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public ReportToppic Topic { get; set; }
    }
}
