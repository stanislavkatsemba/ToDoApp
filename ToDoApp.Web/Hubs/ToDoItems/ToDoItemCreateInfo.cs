using System;

namespace ToDoApp.Web.Hubs.ToDoItems
{
    public class ToDoItemCreateInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? ScheduledDate { get; set; }
    }
}
