namespace Pickpong.Models;

public partial class Tshape
{
    public int IIdShape { get; set; }
    public string NvShapeNameEng { get; set; } = null!;
    public string NvShapeNameHeb { get; set; } = null!;
    public virtual ICollection<TboardSetting> TboardSettings { get; set; } = new List<TboardSetting>();
    public virtual ICollection<TcarpetDetail> TcarpetDetails { get; set; } = new List<TcarpetDetail>();
    public virtual ICollection<Tcustomize> Tcustomizes { get; set; } = new List<Tcustomize>();
}