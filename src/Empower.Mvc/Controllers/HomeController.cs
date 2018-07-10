﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Empower.Mvc.Models;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Empower.Services;

namespace Empower.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private IEmailService _emailService;

        // This is a constructor acting as a recipe.
        // It contains all the ingredients that HomeController
        // needs to do its job.
        //
        public HomeController(
           IEmailService emailService
        )
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            List<Actor> actors = new List<Actor>
            {
                new Actor
                {
                    Firstname = "Meryl",
                    Lastname = "Streep",
                    Films = 20
                },
                new Actor
                {
                    Firstname = "Bobcat",
                    Lastname = "Goldthwaite",
                    Films = 2
                },
                new Actor
                {
                    Firstname = "Steve",
                    Lastname = "Guttenberg",
                    Films = 5
                }
            };

            var vm = new ActorListViewModel
            {
                Actors = actors
            };
            ViewData["actors"] = actors;
            // return View(vm);
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            // New up a ContactViewModel
            var contact = new ContactViewModel();


            return View(contact);
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.CompletedAt = _emailService.SendContactEmail
                (
                    viewModel.Name,
                    viewModel.Email,
                    viewModel.Message
                );

                // We are assigning something to viewModel.ErrorMessage
                viewModel.ErrorMessage =
                    // equivalent of if (viewModel.CompletedAt.HasValue)
                    viewModel.CompletedAt.HasValue
                        // when the above statement is true
                        ? string.Empty
                        // when the above statement is false
                        : "Oops.  We have a problem";

                if (!viewModel.CompletedAt.HasValue)
                {
                    viewModel.ErrorMessage = "Oops.  We have a problem";
                }
            }
            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
