namespace TaskBoardApp.Models
{
    public class BoardAllViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<TaskViewModel> Tasks { get; set; } = new List<TaskViewModel>();
    }
}
