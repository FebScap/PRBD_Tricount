using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_a01.Model;

public class Template : EntityBase<PridContext>
{
    public string Title { get; set; }
    [Required, ForeignKey(nameof(Tricount))]
    public int Tricount { get; set; }
    public int Id { get; set; }
}