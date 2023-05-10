using System;
using System.Collections.Generic;

#nullable disable

namespace NewsWeb.Models
{
    public partial class Account
    {
        public Account()
        {
            Documents = new HashSet<Document>();
        }

        public int AccountId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool Active { get; set; }
        public DateTime? DayCreate { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
