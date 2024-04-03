using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace prbd_2324_a01.Model;

public class Subscription : EntityBase<PridContext>
{
    [Required, ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set;}

    public virtual Tricount Tricount { get; set; }
    public virtual User User { get; set; }

    public Subscription(int user, int tricount) {
        UserId = user;
        TricountId = tricount;
    }

    public Subscription() { }
}