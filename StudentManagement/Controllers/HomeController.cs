using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
        private IStudentRepository _studentRepository;
        private IHostingEnvironment hostingEnvironment;

        public HomeController(IStudentRepository studentRepository,
                              IHostingEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            var model = _studentRepository.GetAllStudent();
            return View(model);
        }

        public ViewResult Details(int? id)
        {
            Student student = _studentRepository.GetStudent(id.Value);

            //Setting up status code if student with certain id does not exist
            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Student = student,
                PageTitle = "Student Details"
            };

            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);
            StudentEditViewModel studentEditViewModel = new StudentEditViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Department = student.Department,
                ExistingPhotoPath = student.PhotoPath
            };
            return View(studentEditViewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _studentRepository.Delete(id);
            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = _studentRepository.GetStudent(model.Id);
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

                string uniqueFileName = ProcessUploadedFile(model);
                Student newStudent = new Student
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _studentRepository.Update(student);
                //redirect to the details of the newly created student
                return RedirectToAction("index", "home");
            }

            return View();
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

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
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
                Student newStudent = new Student
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _studentRepository.Add(newStudent);
                //redirect to the details of the newly created student
                return RedirectToAction("details", new { id = newStudent.Id });
            }

            return View();
        }
    }
}
