using System;

namespace Skybot.Funcs.Models
{
    public class UserAccount
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
    }
}
