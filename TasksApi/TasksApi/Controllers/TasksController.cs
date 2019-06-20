using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TasksApi.Dal;
using TasksApi.Models;

namespace TasksApi.Controllers
{
    public class TasksController : ApiController
    {
        private TasksDBEntities db = new TasksDBEntities();

        // GET: api/Tasks
        public IList<TaskDetail> GetTasks()
        {
            var result = (from task in db.Tasks
                          join parent in db.Parent_Task
                          on task.Parent_ID equals parent.Parent_ID
                          select new TaskDetail()
                          {
                              Task_ID = task.Task_ID,
                              Task = task.Task_Description,
                              Parent_ID = parent.Parent_ID,
                              Parent_Description = parent.Parent_Task_Description,
                              Priority = task.Priority,
                              Start_Date = task.Start_Date,
                              End_Date = task.End_Date
                          }).ToList();
            return result;
        }

        // GET: api/Tasks/5
        [ResponseType(typeof(TaskDetail))]
        public IHttpActionResult GetTask(int id)
        {
            var result = (from task in db.Tasks
                          join parent in db.Parent_Task
                          on task.Parent_ID equals parent.Parent_ID
                          where task.Task_ID == id
                          select new TaskDetail()
                          {
                              Task_ID = task.Task_ID,
                              Task = task.Task_Description,
                              Parent_ID = parent.Parent_ID,
                              Parent_Description = parent.Parent_Task_Description,
                              Priority = task.Priority,
                              Start_Date = task.Start_Date,
                              End_Date = task.End_Date
                          }).ToList();
            
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTask(int id, TaskDetail task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Task_ID)
            {
                return BadRequest();
            }

            Task taskItem = new Task()
            {
                Task_ID = task.Task_ID,
                Task_Description = task.Task,
                Priority = task.Priority,
                Start_Date = task.Start_Date,
                End_Date = task.End_Date
            };

            db.Entry(taskItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tasks
        [ResponseType(typeof(TaskDetail))]
        public IHttpActionResult PostTask(TaskDetail task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Parent_Task parent = new Parent_Task()
            {
                Parent_Task_Description = task.Parent_Description
            };

            db.Parent_Task.Add(parent);

            Task taskItem = new Task()
            {
                Task_Description = task.Task,
                Priority = task.Priority,
                Start_Date = task.Start_Date,
                End_Date = task.End_Date
            };
            db.Tasks.Add(taskItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.Task_ID }, task);
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(Task))]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Task_ID == id) > 0;
        }
    }
}