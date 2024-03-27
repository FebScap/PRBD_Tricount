using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace prbd_2324_a01.Model;

public class Repartition : EntityBase<PridContext>
{
    public int Weight { get; set; }

    [Required, ForeignKey(nameof(Operation))]
    public int OperationId { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public virtual Operation Operation { get; set; }
    public virtual User User { get; set; }

    public Repartition(int weight, int operation, int user) {
        Weight = weight;
        OperationId = operation;
        UserId = user;
    }

    public Repartition() { }
}