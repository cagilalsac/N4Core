﻿#nullable disable

using N4Core;
using N4Core.Records.Bases;

namespace N4Core.Models.Accounts
{
    public class AccountUserModel : Record
    {
        public string UserName { get; set; }
        public List<string> RoleNames { get; set; }
        public string RoleName => RoleNames?.FirstOrDefault();
    }
}
