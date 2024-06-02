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

    public void AddUser () {
        Context.Users.Add(this);
        Context.SaveChanges();
    }
}