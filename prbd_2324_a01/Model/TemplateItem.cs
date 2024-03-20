using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_a01.Model;

public class TemplateItem : EntityBase<PridContext>
{
    public int Weight { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int User { get; set; }

    [Required, ForeignKey(nameof(Template))]
    public int Template { get; set; }
}