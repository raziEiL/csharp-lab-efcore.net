using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models.ViewModels
{
    public class UniversityData
    {
        public IEnumerable<University> Universities { get; set; }
        public IEnumerable<Department> Departments { get; set; }
    }
}