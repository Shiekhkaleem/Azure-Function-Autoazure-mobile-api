using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Accounts
{
    public class LinkedAccount
    {
        public string ID { get; set; }
        public string NickName { get; set; }
        public string Thumbnail { get; set; }
        public DateTime DateLinked { get; set; }
        public int AccountsLimit { get; set; }
        public int SortOrder { get; set; }
        public string Reputation { get; set; }
        public bool Status { get; set; }
        public string Shop_StoreURL { get; set; }
        public string Shop_Token { get; set; }
    }
}
