namespace Pickpong.Models;

public partial class TsysTable
{
    public int IIdSysTable { get; set; }
    public string NvSysTableName { get; set; } = null!;
    public virtual ICollection<TsysTableRow> TsysTableRows { get; set; } = new List<TsysTableRow>();
}