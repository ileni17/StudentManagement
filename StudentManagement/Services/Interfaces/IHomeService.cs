using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Services.Interfaces
{
    public interface IHomeService
    {
        StudentEditViewModel MapStudentToViewModel(Student student);
        Student EditStudent(StudentEditViewModel model);
        Student CreateStudent(StudentCreateViewModel model);
    }
}
