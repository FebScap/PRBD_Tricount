using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_a01.Model;

public class Repartition : EntityBase<PridContext>
{
    public int Weight { get; set; }

    [Required, ForeignKey(nameof(Operation))]
    public int Operation { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int User { get; set; }
    
}