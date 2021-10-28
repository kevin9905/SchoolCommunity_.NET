using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class Advertisement
    {
        [Key]
        public int AdvertisementId
        {
            get;
            set;
        }

        [Required]
        [DisplayName("File Name")]
        public string FileName
        {
            get;
            set;
        }

        [Required]
        [Url]
        public string AdvertisementUrl
        {
            get;
            set;
        }
        [Required]
        [StringLength(10)]
        public string CommunityID { get; set; }
    }
}