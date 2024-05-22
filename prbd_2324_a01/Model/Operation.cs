using PRBD_Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2324_a01.Model;

public class Operation : EntityBase<PridContext>
{
    public int Id { get; set; }
    public string Title { get; set; }

    [Required, ForeignKey(nameof(Tricount))]
    public int Tricount { get; set; }
    public double Amount { get; set; }
    public DateTime OperationDate { get; set; } = DateTime.Now;

    [Required, ForeignKey(nameof(User))]
    public int Initiator {  get; set; }

    public virtual ICollection<Repartition> Repartitions { get; set; } = new HashSet<Repartition>();

    public Operation(string title, int tricount, double amount, int initiator) {
        Title = title;
        Tricount = tricount;
        Amount = amount;
        Initiator = initiator;
    }

    public Operation() { }
    public static IQueryable<Operation> GetAllByTricountId(int id) {
        return Context.Operations.Where(o => o.Tricount == id).OrderByDescending(o => o.OperationDate);
    }

    public static Operation GetLastOperationByTricountId(int id) {
        return GetAllByTricountId(id).FirstOrDefault();
    }
}