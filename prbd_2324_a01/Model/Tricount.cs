using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace prbd_2324_a01.Model;

public class Tricount : EntityBase<PridContext>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required, ForeignKey(nameof(User))]
    public int Creator {  get; set; }
    public int Id { get; set; }

    public virtual ICollection<User> Participants { get; set; } = new HashSet<User>();

    public Tricount(string title, string description, int creator) {
        Title = title;
        Description = description;
        Creator = creator;
    }

    public Tricount() { }

    public static IQueryable<Tricount> GetAllById(int id) {
        return Context.Tricounts.Where(t => t.Creator == id);
    }

    public static IQueryable<Tricount> GetFilteredById(int id, string Filter) {
        var filtered = from t in Context.Tricounts
                       where t.Title.Contains(Filter) || t.Description.Contains(Filter)
                       select t;
        return filtered;
    }
}