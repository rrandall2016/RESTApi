using Core.Models;
using DataStore.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIYouTube.Controllers
{
    //Query is what is in route after ?
    //Route makes this the default http request
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly BugsContext db;

        public ProjectsController(BugsContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.Projects.ToList());
        }
        //This provides a get method with an entered id

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            //Find project with ID, if not, then not ok

            var project = db.Projects.Find(id);

            if (project == null)
            {
                return NotFound();//http 404
            }

            return Ok(project);
        }

        //Using route because not in pattern of the api/controller
        [HttpGet]
        [Route("/api/projects/{pid}/tickets")]
        public IActionResult GetProjectTickets(int pid)
        {
            var tickets = db.Tickets.Where(t => t.ProjectId == pid).ToList();
            if (tickets == null || tickets.Count <= 0)
            {
                return NotFound();
            }
            return Ok(tickets);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Project project)
        {
            db.Projects.Add(project);
            db.SaveChanges();
            return CreatedAtAction(
                    nameof(GetById),
                    new {id = project.ProjectId},
                    project
                );
        }

        //Updating ticket
        [HttpPut("{id}")]
        public IActionResult Put(int id, Project project)
        {
            if (id != project.ProjectId) return BadRequest();

            db.Entry(project).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                //deleted before saved

                if (db.Projects.Find(id) == null)
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var project = db.Projects.Find(id);
            if (project == null) return NotFound();

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }
    }
}
