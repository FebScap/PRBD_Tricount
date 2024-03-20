using PRBD_Framework;

namespace prbd_2324_a01.Model;

public class Templates : EntityBase<PridContext>
{
    public string Title { get; set; }
    public int Tricount { get; set; }
    public int Id { get; set; }
}