using LabBigSchool_TruongTheHao.Models;
using LabBigSchool_TruongTheHao.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabBigSchool_TruongTheHao.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Course

        private readonly ApplicationDBContext _dBContext;
        public CoursesController()
        {
            _dBContext = new ApplicationDBContext();
        }
            
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dBContext.Categories.ToList()
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dBContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                Datetime = viewModel.GetDateTime(),
                CategoryId = viewModel.Category,
                Place = viewModel.Place
            };
            _dBContext.Courses.Add(course);
            _dBContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}