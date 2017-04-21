using System;
using System.Collections.Generic;
using System.Text;

namespace WeiChat.Models
{
    public class InsuranceModel
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public string Name { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string InsType { get; set; }
    }
}
