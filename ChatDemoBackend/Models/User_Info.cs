using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatDemoBackend.Models
{
    public class User_Info
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string User_Password { get; set; }

        public DateTime? Register_Date { get; set; }



        public override string ToString()
        {
            return UserId+" - "+UserName+" - "+User_Password+" - "+Register_Date;
        }

    }
}
