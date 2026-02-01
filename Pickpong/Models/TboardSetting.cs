namespace Pickpong.Models;

public partial class TboardSetting
{
    public int IIdSize { get; set; }

    public int IIdShape { get; set; }

    public decimal DLength { get; set; }

    public decimal DHeight { get; set; }

    public decimal DPrice { get; set; }

    public int IMaxPlayers { get; set; }

    public virtual Tshape IIdShapeNavigation { get; set; } = null!;
}