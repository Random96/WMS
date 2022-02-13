using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ru.EmlSoft.WMS.Data.Abstract.Identity
{
    public class Logins
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int Result { get; set; }
        public string PasswordHash { get; set; }
        public virtual User User { get; set; }
    }
}
