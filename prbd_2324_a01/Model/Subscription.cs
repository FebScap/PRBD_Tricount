using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_a01.Model;

public class Subscription : EntityBase<PridContext>
{
    [Required, ForeignKey(nameof(Tricount))]
    public int Tricount { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int User { get; set;}
}