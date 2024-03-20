using PRBD_Framework;

namespace prbd_2324_a01.Model;

public class User : EntityBase<PridContext>
{
    public int UserId { get; set; }
    public string Mail { get; set; }
    public string HashedPassword { get; set; }
    public string FullName { get; set; }
    public int Role {  get; set; }
}