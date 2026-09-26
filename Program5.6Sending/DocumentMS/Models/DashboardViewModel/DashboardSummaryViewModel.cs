using System;
using System.Collections.Generic;

namespace DocumentMS.Models.DashboardViewModel
{
    public class DashboardSummaryViewModel
    {
        public int TotalDocument { get; set; }
        public int TotalDocCategories { get; set; }
        public int TotalComment { get; set; }
        public int TotalDocumentStatus { get; set; }


        public Int64 TotalUser { get; set; }
        public Int64 TotalActive { get; set; }
        public Int64 TotalInActive { get; set; }
        public List<UserProfile> listUserProfile { get; set; }
    }
}
