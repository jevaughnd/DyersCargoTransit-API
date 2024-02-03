﻿using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_Interface.Models
{
    public class CustomerProfileDto
    {

        public string Username { get; set; }
        public string Password { get; set; }


        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }


        public int ParishId { get; set; }
        [ForeignKey("ParishId")]
        public virtual Parish? Parish { get; set; }

        public IFormFile? ProfilePictureFile { get; set; }
    }
}
