using System.ComponentModel.DataAnnotations.Schema;

public class ItemData
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Details { get; set; }
    public bool Done { get; set; }
    //public object Completed { get; internal set; } 
}


