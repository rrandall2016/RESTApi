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
    //Not MVC Controller, it's an Api Controller attribute 
    //Contains related functions 

    //Model validation happens automatically within filter pipeline using this attribute APIController
    //
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly BugsContext db;

        //Create Constructor
        //DI
        public TicketsController(BugsContext db)
        {
            this.db = db;
        }

        //IActionResult can return multiple data types, JSON, XSL
        //Return all the tickets
        //Routes to api/tickets with route attributes
        [HttpGet]
       
        public IActionResult Get()
        {
            
            //To list forces execution 
            //Tickets are in BugsContext/DataStore.EF

            return Ok(db.Tickets.ToList());
        }

        [HttpGet("{id}")]
       
        public IActionResult GetById(int id)
        {
            var ticket = db.Tickets.Find(id);
            if(ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }
        //Create method
        //Using version 1 and version 2 with Action Filter because version 1 wants only DueDate, but vs wants Enter date as well,
        //cant use Data Annotations for this validation because it will apply this validation to all methods and break version 1
        [HttpPost]
        
        public IActionResult Post([FromBody]Ticket ticket)
        {
            db.Tickets.Add(ticket);
            db.SaveChanges();
            //Provides new uri for caller
            
            return CreatedAtAction(
                nameof(GetById),
                new {id = ticket.TicketId},
                ticket
                );
        }
        //Apply the attribute we made as an action filter with [ ticket_ensure...]
        //To force Web API to read a simple type from the request body,
        //add the [FromBody] attribute to the parameter:

      

        //Updating ticket
        [HttpPut("{id}")]
        public IActionResult Put(int id, Ticket ticket)
        {
            //Ticket and ticket id are same? 
            if (id != ticket.TicketId) return BadRequest();

            //Modify state of the change tracker
            //db save changes, knows it needs to generate update statement
            //Updates all fields
            db.Entry(ticket).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch
            {
                //Does ticket exist? 
                if (db.Tickets.Find(id) == null)
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Find ticket
            var ticket = db.Tickets.Find(id);
            //if ticket empty, not found
            if (ticket == null) return NotFound();

            //Remove this ticket, marks at deleted
            db.Tickets.Remove(ticket);

            db.SaveChanges();

            //returns deleted ticket
            return Ok(ticket);
        }

    }
}
