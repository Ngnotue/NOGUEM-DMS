using System;

namespace DocumentMS.Models
{
    public class EmailConfig : EntityBase
    {
        public Int64 Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string SenderFullName { get; set; }
    }
}
