using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TasksApi.Models
{
    public class TaskDetail
    {
        public int Task_ID { get; set; }
        public Nullable<int> Parent_ID { get; set; }
        public string Parent_Description { get; set; }
        public string Task { get; set; }
        public Nullable<System.DateTime> Start_Date { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public Nullable<int> Priority { get; set; }
    }
}