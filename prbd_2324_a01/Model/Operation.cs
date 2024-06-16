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

    public User GetInitiator() {
        return Context.Users.Find(this.Initiator);
    }
    
    private double GetTotalWeight() {
            return Repartitions.Sum(r => r.Weight);
    }
    public Dictionary<int, double> GetParticipantShares() {
        var totalWeight = GetTotalWeight();
        var shares = new Dictionary<int, double>();

        foreach (var repartition in Repartitions) {
            var user = repartition.User.Id;
            var share = (repartition.Weight / totalWeight) * Amount;
            shares[user] = share;
        }
        return shares;
    }

    public int getUserWeight(int userId) {
        return Context.Repartitions.Where(r => r.UserId == userId && r.OperationId == this.Id).Select(r => r.Weight).FirstOrDefault();
    }

    public void Add() {
        Context.Operations.Add(this);
        Context.SaveChanges();
    }

    public void Update() {
        Context.Operations.Update(this);
        Context.SaveChanges();
    }

    public void UpdateBalance(Dictionary<int, int> balance) {
        foreach (var rep in balance) {
            if (Context.Repartitions.Find(this.Id, rep.Key) != null) {
                Repartition r = Context.Repartitions.Find(this.Id, rep.Key);
                if (rep.Value == 0) {
                    Context.Repartitions.Remove(r);
                } else {
                    r.Weight = rep.Value;
                    Context.Repartitions.Update(r);
                }
            } else {
                if (rep.Value != 0) Context.Repartitions.Add(new Repartition(this.Id, rep.Key, rep.Value)); 
            }
        }
        Context.SaveChanges();
    }

    public Tricount GetTricount() {
        return Context.Tricounts.Find(this.Tricount);
    }

    internal void Delete() {
        Context.Operations.Remove(this);
        Context.SaveChanges();
    }
}