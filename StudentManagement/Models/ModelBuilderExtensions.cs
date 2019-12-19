using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FirstName = "Mary",
                    LastName = "Poppins",
                    Department = Department.English,
                    Email = "marypoppins@school.com"
                },
                new Student
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Wayne",
                    Department = Department.History,
                    Email = "johnwayne@school.com"
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Bruce",
                    LastName = "Lee",
                    Department = Department.PE,
                    Email = "brucelee@school.com"
                }
            );
        }
    }
}
