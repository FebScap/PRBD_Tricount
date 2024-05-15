using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace prbd_2324_a01.Model;

public class Subscription : EntityBase<PridContext>
{
    [ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    public virtual Tricount Tricount { get; set; }


    [ForeignKey(nameof(User))]
    public int UserId { get; set;}
    public virtual User User { get; set; }

    public Subscription(int user, int tricount) {
        UserId = user;
        TricountId = tricount;
    }

    public Subscription() { }

    public static List<Tricount> GetAllTricountByUserId(int id) {
        List<Tricount> tricounts = new List<Tricount>();
        foreach (Subscription sub in Context.Subscriptions.Where(s => s.UserId == id)) {
            tricounts.Add(Context.Tricounts.Find(sub.TricountId));
        }
        return tricounts;
    }

    public static List<User> GetAllUserByTricountIdExeptCurent(int id, User current) {
        List<User> users = new List<User>();
        foreach (Subscription sub in Context.Subscriptions.Where(s => s.TricountId == id)) {
            if (sub.UserId != current.Id) {
                users.Add(Context.Users.Find(sub.UserId));
            }
        }
        return users;
    }
}