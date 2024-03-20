using PRBD_Framework;

namespace prbd_2324_a01.Model;

public class TemplateItems : EntityBase<PridContext>
{
    public int Weight { get; set; }
    public int User { get; set; }
    public int Template { get; set; }
}