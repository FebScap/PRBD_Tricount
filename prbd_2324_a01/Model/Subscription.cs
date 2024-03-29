using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace prbd_2324_a01.Model;

public class Subscription : EntityBase<PridContext>
{
    [ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    public virtual Tricount Tricount { get; set; }


    [ForeignKey(nameof(User))]
    public int UserId { get; set;}
    public virtual User User { get; set; }

    public Subscription(int tricount, int user) {
        TricountId = tricount;
        UserId = user;
    }

    public Subscription() { }
}