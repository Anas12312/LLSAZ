using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto.UserDto
{
    public class StudentListDto
    {
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "Student";
        public string Status { get; set; } //Active-NotActive
        public DateTime Updated { get; set; }//how many minutes ago .. ex: 18 minutes ago
        // public DateTime LastLogin { get; set; }
    }
}
