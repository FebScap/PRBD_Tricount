using PRBD_Framework;

namespace prbd_2324_a01.Model;

public class User : EntityBase<PridContext>
{
    public int Id { get; set; }
    public string Mail { get; set; }
    public string HashedPassword { get; set; }
    public string FullName { get; set; }
    public int Role {  get; set; }


    public virtual ICollection<Tricount> Tricounts { get; set; } = new HashSet<Tricount>();
    public virtual ICollection<Repartition> Repartitions { get; set; } = new HashSet<Repartition>();

    public User(string mail, string hashedPassword, string fullName, int role) {
        Mail = mail;
        HashedPassword = hashedPassword;
        FullName = fullName;
        Role = role;
    }

    public User() {}

    public void Add () {
        Context.Users.Add(this);
        Context.SaveChanges();
    }

    public IQueryable<Tricount> GetAllOwnedTricount() {
        return Context.Tricounts.Where(t => t.Creator == this.Id);
    }

    public bool IsTitleUnique(string title) {
        Console.WriteLine("unique");
        var Tricounts = GetAllOwnedTricount();
        foreach (var tricount in Tricounts) {
            if (tricount.Title == title) return false;
        }
        return true;
    }
}