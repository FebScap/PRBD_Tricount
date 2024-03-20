using PRBD_Framework;

namespace prbd_2324_a01.Model;

public class Repartitions : EntityBase<PridContext>
{
    public int Weight { get; set; }
    public int Operation { get; set; }
    public int User { get; set; }
    
}