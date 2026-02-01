namespace Pickpong.Models;

public partial class Tcolor
{
    public int IIdColor { get; set; }

    public string NvColorNameEng { get; set; } = null!;

    public string NvColorNameHeb { get; set; } = null!;

    public virtual ICollection<TcarpetDetail> TcarpetDetails { get; set; } = new List<TcarpetDetail>();
}