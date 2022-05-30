using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LabBigSchool_TruongTheHao.Models
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public ApplicationDBContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDBContext Create()
        {
            return new ApplicationDBContext();
        }
    }
}