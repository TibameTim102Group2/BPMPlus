using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BPMPlus.Data;
using BPMPlus.Models;

namespace BPMPlus.Controllers
{
    public class CreateProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CreateProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CreateProjects
        public async Task<IActionResult> Index()
        {
            return View();
        }

     
    }
}
