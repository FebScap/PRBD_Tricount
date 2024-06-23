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
        var Tricounts = GetAllOwnedTricount();
        foreach (var tricount in Tricounts) {
            if (tricount.Title == title) return false;
        }
        return true;
    }

    public List<Tricount> GetAllTricount() {
        List<Tricount> tricounts = new List<Tricount>();

        foreach (Subscription sub in Context.Subscriptions.Where(s => s.UserId == this.Id)) {
            tricounts.Add(Context.Tricounts.Find(sub.TricountId));
        }

        // Tri par ordre chronologique inverse
        var sortedTricounts = tricounts
        .OrderByDescending(t => t.CreatedAt)
        .ThenByDescending(t => t.GetLastOperation() != null ? t.GetLastOperationDate() : new DateTime())
        .ToList();

        return sortedTricounts;
    }

    public List<Tricount> GetAllTricountFiltered(string filter) {
        var tricounts = new List<Tricount>();

        var titleList = Context.Operations.Where(o => o.Title.Contains(filter)).Select(s => s.Tricount);
        var filteredSubscriptions = Context.Subscriptions
            .Where(s => s.UserId == this.Id &&
                        (s.Tricount.Title.Contains(filter) ||
                         s.Tricount.Description.Contains(filter) ||
                         s.Tricount.Participants.Any(p => p.FullName.Contains(filter)) ||
                         titleList.Contains(s.Tricount.Id))) 
            .Select(s => s.Tricount);

        tricounts.AddRange(filteredSubscriptions);

        // Tri par ordre chronologique inverse
        var sortedTricounts = tricounts
         .OrderByDescending(t => t.CreatedAt)
         .ThenByDescending(t => t.GetLastOperation() != null ? t.GetLastOperationDate() : new DateTime())
         .ToList();

        return sortedTricounts;
    }


    public static User GetUserById(int id) {
        using (var context = new PridContext()) {
            var user = context.Users
                .Where(u => u.Id == id)
                .Select(u => new User {
                    Id = u.Id,
                    Mail = u.Mail,
                    HashedPassword = u.HashedPassword,
                    FullName = u.FullName,
                    Role = u.Role
                })
                .SingleOrDefault();

            return user;
        }
    }

    public override string ToString() {
        return $"User Id: {Id}, Mail: {Mail}, Full Name: {FullName}, Role: {Role}";
    }

}