using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services.Interfaces;
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
        private IStudentRepository studentRepository;
        private IHostingEnvironment hostingEnvironment;
        private IHomeService homeService;

        public HomeController(IStudentRepository studentRepository, IHostingEnvironment hostingEnvironment, IHomeService homeService)
        {
            this.studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.homeService = homeService;
        }

        public ViewResult Index()
        {
            var model = studentRepository.GetAllStudent();
            return View(model);
        }

        public ViewResult Details(int? id)
        {
            Student student = studentRepository.GetStudent(id.Value);

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
            Student student = studentRepository.GetStudent(id);
            StudentEditViewModel studentEditViewModel = homeService.MapStudentToViewModel(student);
            return View(studentEditViewModel);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            studentRepository.Delete(id);
            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = homeService.EditStudent(model);
                studentRepository.Update(student);
                //redirect to the home page
                return RedirectToAction("index", "home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student newStudent = homeService.CreateStudent(model);

                studentRepository.Add(newStudent);
                //redirect to the details of the newly created student
                return RedirectToAction("details", new { id = newStudent.Id });
            }

            return View();
        }
    }
}
