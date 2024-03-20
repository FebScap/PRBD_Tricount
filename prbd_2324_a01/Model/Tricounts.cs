using PRBD_Framework;

namespace prbd_2324_a01.Model;

public class Tricounts : EntityBase<PridContext>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Creator {  get; set; }
    public int Id { get; set; }
}