using LabBigSchool_TruongTheHao.Models;
using LabBigSchool_TruongTheHao.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabBigSchool_TruongTheHao.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Course

        private readonly ApplicationDbContext _dBContext;
        public CoursesController()
        {
            _dBContext = new ApplicationDbContext();
        }
            
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dBContext.Categories.ToList(),
                Heading = "Add Course"
            };
            return View("CourseForm",viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dBContext.Categories.ToList();
                return View("CourseForm", viewModel);
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

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dBContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.Category)
                .ToList();

            var viewModel = new CourseViewModel
            {
                UpcomingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dBContext.Courses
                .Where(c => c.LecturerId == userId && c.Datetime > DateTime.Now)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();
            return View(courses);
        }


        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var course = _dBContext.Courses.Single(c => c.Id == id && c.LecturerId == userId);

            var viewModel = new CourseViewModel
            {
                Categories = _dBContext.Categories.ToList(),
                Date = course.Datetime.ToString("dd/M/yyyy"),
                Time = course.Datetime.ToString("HH:mm"),
                Category = course.CategoryId,
                Place = course.Place,
                Heading = "Edit Course",
                Id = course.Id
            };
            return View("CourseForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update (CourseViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Categories = _dBContext.Categories.ToList();
                return View("CourseForm", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var course = _dBContext.Courses.Single(c => c.Id == viewModel.Id && c.LecturerId == userId);

            course.Place = viewModel.Place;
            course.Datetime = viewModel.GetDateTime();
            course.CategoryId = viewModel.Category;

            _dBContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}