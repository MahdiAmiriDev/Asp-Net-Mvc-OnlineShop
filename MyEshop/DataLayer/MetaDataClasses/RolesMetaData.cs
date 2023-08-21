using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    class RolesMetaData
    {
        [Key]
        [Display(Name ="شناسه نقش")]
        public int RoleID { get; set; }
        [Display(Name ="عنوان نقش")]
        [Required(ErrorMessage ="لطفا مقدار {0} را وارد کنید")]
        public string RoleTitle { get; set; }
        [Display(Name ="عنوان سیستمی نقش")]
        [Required(ErrorMessage ="لطفا مقدار {0} را وارد کنید")]
        public string RoleName { get; set; }

    }
}
