using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_a01.Model;

public class Tricount : EntityBase<PridContext>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required, ForeignKey(nameof(User))]
    public int Creator {  get; set; }
    public int Id { get; set; }

    public Tricount(string title, string description, int creator) {
        Title = title;
        Description = description;
        Creator = creator;
    }

    public Tricount() { }
}