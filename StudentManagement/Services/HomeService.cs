using Microsoft.AspNetCore.Hosting;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class HomeService : IHomeService
    {
        private IStudentRepository studentRepository;
        private IHostingEnvironment hostingEnvironment;

        public HomeService(IStudentRepository studentRepository,
                              IHostingEnvironment hostingEnvironment)
        {
            this.studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        public Student CreateStudent(StudentCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //ensure that uploads with same name are uploaded as different files
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                //ensure that file path guides to the folder where we want to save our uploads
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return new Student()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Department = model.Department,
                PhotoPath = uniqueFileName
            };
        }

        public Student EditStudent(StudentEditViewModel model)
        {
            Student student = studentRepository.GetStudent(model.Id);
            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.Email = model.Email;
            student.Department = model.Department;
            if (model.Photo != null)
            {
                //Check if user uploaded photo and its location
                if (model.ExistingPhotoPath != null)
                {
                    string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                    //Delete photo
                    System.IO.File.Delete(filePath);
                }
                student.PhotoPath = ProcessUploadedFile(model);
            }
            return student;
        }

        public StudentEditViewModel MapStudentToViewModel(Student student)
        {
            return new StudentEditViewModel()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Department = student.Department,
                ExistingPhotoPath = student.PhotoPath
            };
        }

        private string ProcessUploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //ensure that uploads with same name are uploaded as different files
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                //ensure that file path guides to the folder where we want to save our uploads
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
