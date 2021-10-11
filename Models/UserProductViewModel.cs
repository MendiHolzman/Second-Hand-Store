using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspProject.Models
{
    public class UserProductViewModel
    {
        public UserProductViewModel()
        {

        }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
