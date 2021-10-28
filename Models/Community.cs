using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Community
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//Makes sure this is not database generated
        [DisplayName("Registration Number")]
        [Required]
        public string id
        {
            get;
            set;
        }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Must be 3 to 50 characters")]
        public string Title
        {
            get;
            set;
        }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget
        {
            get;
            set;
        }

        public virtual IEnumerable<CommunityMembership> CommunityMemberships { get; set; }
    }
}
