using PRBD_Framework;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_a01.Model;

public class Operations : EntityBase<PridContext>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Tricount { get; set; }
    public double Amount { get; set; }
    public DateTime OperationDate { get; set; }
    public int Initiator {  get; set; }
}