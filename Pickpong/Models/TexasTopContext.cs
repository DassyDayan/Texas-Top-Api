using Microsoft.EntityFrameworkCore;

namespace Pickpong.Models;

public partial class TexasTopContext : DbContext
{
    public TexasTopContext() { }
    public TexasTopContext(DbContextOptions<TexasTopContext> options) : base(options) { }
    public virtual DbSet<TboardSetting> TboardSettings { get; set; }

    public virtual DbSet<TcarpetDetail> TcarpetDetails { get; set; }

    public virtual DbSet<TcarpetDetailsInCart> TcarpetDetailsInCarts { get; set; }

    public virtual DbSet<TcartDetail> TcartDetails { get; set; }

    public virtual DbSet<Tcolor> Tcolors { get; set; }

    public virtual DbSet<TcustomerDetail> TcustomerDetails { get; set; }

    public virtual DbSet<Tcustomize> Tcustomizes { get; set; }

    public virtual DbSet<TgeneralSetting> TgeneralSettings { get; set; }

    public virtual DbSet<TinvoiceDetail> TinvoiceDetails { get; set; }

    public virtual DbSet<Torder> Torders { get; set; }

    public virtual DbSet<Tplayer> Tplayers { get; set; }

    public virtual DbSet<Tshape> Tshapes { get; set; }

    public virtual DbSet<TsysTable> TsysTables { get; set; }

    public virtual DbSet<TsysTableRow> TsysTableRows { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer
        ("Server=109.226.37.218;Database=TexasTop;User Id=sa;Password=1qaZXsw2;TrustServerCertificate=True;Trusted_Connection=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TboardSetting>(entity =>
        {
            entity.HasKey(e => e.IIdSize).HasName("PK__TBoardSe__C890FB8766603565");

            entity.ToTable("TBoardSettings");

            entity.Property(e => e.IIdSize).HasColumnName("iIdSize");
            entity.Property(e => e.DHeight)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dHeight");
            entity.Property(e => e.DLength)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dLength");
            entity.Property(e => e.DPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dPrice");
            entity.Property(e => e.IIdShape).HasColumnName("iIdShape");
            entity.Property(e => e.IMaxPlayers).HasColumnName("iMaxPlayers");

            entity.HasOne(d => d.IIdShapeNavigation).WithMany(p => p.TboardSettings)
                .HasForeignKey(d => d.IIdShape)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TBoardSet__iIdSh__68487DD7");
        });

        modelBuilder.Entity<TcarpetDetail>(entity =>
        {
            entity.HasKey(e => e.IIdCarpet).HasName("PK__TCarpetD__E055B9BA173876EA");

            entity.ToTable("TCarpetDetails");

            entity.Property(e => e.IIdCarpet).HasColumnName("iIdCarpet");
            entity.Property(e => e.BPutGuestOrEmptyPlace).HasColumnName("bPutGuestOrEmptyPlace");
            entity.Property(e => e.BWithNamesOrNot).HasColumnName("bWithNamesOrNot");
            entity.Property(e => e.FSizeParameterA).HasColumnName("fSizeParameterA");
            entity.Property(e => e.FSizeParameterB).HasColumnName("fSizeParameterB");
            entity.Property(e => e.IIdColor).HasColumnName("iIdColor");
            entity.Property(e => e.IIdShape).HasColumnName("iIdShape");
            entity.Property(e => e.INumOfPlayers).HasColumnName("iNumOfPlayers");
            entity.Property(e => e.NvCarpetFileName)
                .HasMaxLength(100)
                .HasColumnName("nvCarpetFileName");
            entity.Property(e => e.NvGroupName)
                .HasMaxLength(500)
                .HasColumnName("nvGroupName");
            entity.Property(e => e.NvLogoFileName)
                .HasMaxLength(100)
                .HasColumnName("nvLogoFileName");
            entity.Property(e => e.NvSmallCarpetFileName)
                .HasMaxLength(100)
                .HasColumnName("nvSmallCarpetFileName");

            entity.HasOne(d => d.IIdColorNavigation).WithMany(p => p.TcarpetDetails)
                .HasForeignKey(d => d.IIdColor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TCarpetDe__iIdCo__1A14E395");

            entity.HasOne(d => d.IIdShapeNavigation).WithMany(p => p.TcarpetDetails)
                .HasForeignKey(d => d.IIdShape)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TCarpetDe__iIdSh__1920BF5C");
        });

        modelBuilder.Entity<TcarpetDetailsInCart>(entity =>
        {
            entity.HasKey(e => e.IIdCarpetDetailsInCart).HasName("PK__TCarpetD__930C44A66EF57B66");

            entity.ToTable("TCarpetDetailsInCart");

            entity.Property(e => e.IIdCarpetDetailsInCart).HasColumnName("iIdCarpetDetailsInCart");
            entity.Property(e => e.BDeleted).HasColumnName("bDeleted");
            entity.Property(e => e.ICount)
                .HasDefaultValueSql("((1))")
                .HasColumnName("iCount");
            entity.Property(e => e.IIdCarpet).HasColumnName("iIdCarpet");
            entity.Property(e => e.IIdCart).HasColumnName("iIdCart");

            entity.HasOne(d => d.IIdCarpetNavigation).WithMany(p => p.TcarpetDetailsInCarts)
                .HasForeignKey(d => d.IIdCarpet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TCarpetDe__iIdCa__70DDC3D8");

            entity.HasOne(d => d.IIdCartNavigation).WithMany(p => p.TcarpetDetailsInCarts)
                .HasForeignKey(d => d.IIdCart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TCarpetDe__iIdCa__71D1E811");
        });

        modelBuilder.Entity<TcartDetail>(entity =>
        {
            entity.HasKey(e => e.IIdCart).HasName("PK__TCartDet__4CAE4CAC2A4B4B5E");

            entity.ToTable("TCartDetails");

            entity.Property(e => e.IIdCart).HasColumnName("iIdCart");
            entity.Property(e => e.BStatusPaid).HasColumnName("bStatusPaid");
            entity.Property(e => e.IIdCustomer).HasColumnName("iIdCustomer");

            entity.HasOne(d => d.IIdCustomerNavigation).WithMany(p => p.TcartDetails)
                .HasForeignKey(d => d.IIdCustomer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TCartDeta__iIdCu__2C3393D0");
        });

        modelBuilder.Entity<Tcolor>(entity =>
        {
            entity.HasKey(e => e.IIdColor).HasName("PK__TColors__C17DA5391367E606");

            entity.ToTable("TColors");

            entity.Property(e => e.IIdColor).HasColumnName("iIdColor");
            entity.Property(e => e.NvColorNameEng)
                .HasMaxLength(50)
                .HasColumnName("nvColorNameEng");
            entity.Property(e => e.NvColorNameHeb)
                .HasMaxLength(50)
                .HasColumnName("nvColorNameHeb");
        });

        modelBuilder.Entity<TcustomerDetail>(entity =>
        {
            entity.HasKey(e => e.IIdCustomer).HasName("PK__TCustome__55145E5522AA2996");

            entity.ToTable("TCustomerDetails");

            entity.Property(e => e.IIdCustomer).HasColumnName("iIdCustomer");
            entity.Property(e => e.IBuildingNumber).HasColumnName("iBuildingNumber");
            entity.Property(e => e.IDepartmentNumber).HasColumnName("iDepartmentNumber");
            entity.Property(e => e.ISysTableRowIdCity)
                .HasMaxLength(50)
                .HasColumnName("iSysTableRowIdCity");
            entity.Property(e => e.ISysTableRowIdEarth)
                .HasMaxLength(50)
                .HasColumnName("iSysTableRowIdEarth");
            entity.Property(e => e.ISysTableRowIdPrefix)
                .HasMaxLength(50)
                .HasColumnName("iSysTableRowIdPrefix");
            entity.Property(e => e.ISysTableRowIdStreet)
                .HasMaxLength(50)
                .HasColumnName("iSysTableRowIdStreet");
            entity.Property(e => e.NvEmailAddress)
                .HasMaxLength(50)
                .HasColumnName("nvEmailAddress");
            entity.Property(e => e.NvFirstName)
                .HasMaxLength(50)
                .HasColumnName("nvFirstName");
            entity.Property(e => e.NvLastName)
                .HasMaxLength(50)
                .HasColumnName("nvLastName");
            entity.Property(e => e.NvPhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("nvPhoneNumber");
            entity.Property(e => e.NvShippingNote)
                .HasMaxLength(200)
                .HasColumnName("nvShippingNote");
            entity.Property(e => e.NvZipCode)
                .HasMaxLength(50)
                .HasColumnName("nvZipCode");
        });

        modelBuilder.Entity<Tcustomize>(entity =>
        {
            entity.HasKey(e => e.IIdCost).HasName("PK__TCustomi__43FA89664F7CD00D");

            entity.ToTable("TCustomize");

            entity.Property(e => e.IIdCost).HasColumnName("iIdCost");
            entity.Property(e => e.DMaxHeight)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dMaxHeight");
            entity.Property(e => e.DMaxLength)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dMaxLength");
            entity.Property(e => e.DMinHeight)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dMinHeight");
            entity.Property(e => e.DMinLength)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dMinLength");
            entity.Property(e => e.DPriceForMeter)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dPriceForMeter");
            entity.Property(e => e.IIdShape).HasColumnName("iIdShape");

            entity.HasOne(d => d.IIdShapeNavigation).WithMany(p => p.Tcustomizes)
                .HasForeignKey(d => d.IIdShape)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TCustomiz__iIdSh__5165187F");
        });

        modelBuilder.Entity<TgeneralSetting>(entity =>
        {
            entity.HasKey(e => e.IIdGeneralSetting).HasName("PK__TGeneral__86E31EC26B24EA82");

            entity.ToTable("TGeneralSettings");

            entity.Property(e => e.IIdGeneralSetting).HasColumnName("iIdGeneralSetting");
            entity.Property(e => e.DValue)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("dValue");
            entity.Property(e => e.NvKey)
                .HasMaxLength(50)
                .HasColumnName("nvKey");
        });

        modelBuilder.Entity<TinvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.IIdInvoice).HasName("PK__TInvoice__14BFDB6337A5467C");

            entity.ToTable("TInvoiceDetails");

            entity.Property(e => e.IIdInvoice).HasColumnName("iIdInvoice");
            entity.Property(e => e.IBuildingNumber).HasColumnName("iBuildingNumber");
            entity.Property(e => e.IDepartmentNumber).HasColumnName("iDepartmentNumber");
            entity.Property(e => e.ISysTableRowIdCity)
                .HasMaxLength(50)
                .HasColumnName("iSysTableRowIdCity");
            entity.Property(e => e.ISysTableRowIdEarth)
                .HasMaxLength(50)
                .HasColumnName("iSysTableRowIdEarth");
            entity.Property(e => e.ISysTableRowIdStreet)
                .HasMaxLength(50)
                .HasColumnName("iSysTableRowIdStreet");
            entity.Property(e => e.NvDealerName)
                .HasMaxLength(50)
                .HasColumnName("nvDealerName");
            entity.Property(e => e.NvDealerNumber)
                .HasMaxLength(50)
                .HasColumnName("nvDealerNumber");
            entity.Property(e => e.NvEmailAddress)
                .HasMaxLength(50)
                .HasColumnName("nvEmailAddress");
            entity.Property(e => e.NvPhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("nvPhoneNumber");
            entity.Property(e => e.NvZipCode)
                .HasMaxLength(50)
                .HasColumnName("nvZipCode");
        });

        modelBuilder.Entity<Torder>(entity =>
        {
            entity.HasKey(e => e.IIdOrder).HasName("PK__TOrder__40E8D7415DCAEF64");

            entity.ToTable("TOrder");

            entity.Property(e => e.IIdOrder).HasColumnName("iIdOrder");
            entity.Property(e => e.DPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("dPrice");
            entity.Property(e => e.DtOrderDate)
                .HasColumnType("datetime")
                .HasColumnName("dtOrderDate");
            entity.Property(e => e.IIdCart).HasColumnName("iIdCart");
            entity.Property(e => e.IIdInvoice).HasColumnName("iIdInvoice");
            entity.Property(e => e.ISysTableRowIdOrderStatus).HasColumnName("iSysTableRowIdOrderStatus");
            entity.Property(e => e.ISysTableRowIdPaymentType).HasColumnName("iSysTableRowIdPaymentType");
            entity.Property(e => e.ISysTableRowIdShippingType).HasColumnName("iSysTableRowIdShippingType");
            entity.Property(e => e.NvOrderNumber)
                .HasMaxLength(50)
                .HasColumnName("nvOrderNumber");
            entity.Property(e => e.NvShippingNote)
                .HasMaxLength(200)
                .HasColumnName("nvShippingNote");

            entity.HasOne(d => d.IIdCartNavigation).WithMany(p => p.Torders)
                .HasForeignKey(d => d.IIdCart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TOrder__iIdCart__5FB337D6");

            entity.HasOne(d => d.IIdInvoiceNavigation).WithMany(p => p.Torders)
                .HasForeignKey(d => d.IIdInvoice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TOrder__iIdInvoi__628FA481");

            entity.HasOne(d => d.ISysTableRowIdOrderStatusNavigation).WithMany(p => p.TorderISysTableRowIdOrderStatusNavigations)
                .HasForeignKey(d => d.ISysTableRowIdOrderStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TOrder__iSysTabl__619B8048");

            entity.HasOne(d => d.ISysTableRowIdPaymentTypeNavigation).WithMany(p => p.TorderISysTableRowIdPaymentTypeNavigations)
                .HasForeignKey(d => d.ISysTableRowIdPaymentType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TOrder__iSysTabl__6383C8BA");

            entity.HasOne(d => d.ISysTableRowIdShippingTypeNavigation).WithMany(p => p.TorderISysTableRowIdShippingTypeNavigations)
                .HasForeignKey(d => d.ISysTableRowIdShippingType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TOrder__iSysTabl__60A75C0F");
        });

        modelBuilder.Entity<Tplayer>(entity =>
        {
            entity.HasKey(e => e.IIdPlayer).HasName("PK__TPlayers__0FFAE0531CF15040");

            entity.ToTable("TPlayers");

            entity.Property(e => e.IIdPlayer).HasColumnName("iIdPlayer");
            entity.Property(e => e.IIdCarpet).HasColumnName("iIdCarpet");
            entity.Property(e => e.IPlace).HasColumnName("iPlace");
            entity.Property(e => e.NvName)
                .HasMaxLength(50)
                .HasColumnName("nvName");
            entity.Property(e => e.NvNickName)
                .HasMaxLength(50)
                .HasColumnName("nvNickName");

            entity.HasOne(d => d.IIdCarpetNavigation).WithMany(p => p.Tplayers)
                .HasForeignKey(d => d.IIdCarpet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TPlayers__iIdCar__1ED998B2");
        });

        modelBuilder.Entity<Tshape>(entity =>
        {
            entity.HasKey(e => e.IIdShape).HasName("PK__TShapes__369AE0EA0F975522");

            entity.ToTable("TShapes");

            entity.Property(e => e.IIdShape).HasColumnName("iIdShape");
            entity.Property(e => e.NvShapeNameEng)
                .HasMaxLength(50)
                .HasColumnName("nvShapeNameEng");
            entity.Property(e => e.NvShapeNameHeb)
                .HasMaxLength(50)
                .HasColumnName("nvShapeNameHeb");
        });

        modelBuilder.Entity<TsysTable>(entity =>
        {
            entity.HasKey(e => e.IIdSysTable).HasName("PK__TSysTabl__103FDDDA07020F21");

            entity.ToTable("TSysTables");

            entity.Property(e => e.IIdSysTable).HasColumnName("iIdSysTable");
            entity.Property(e => e.NvSysTableName)
                .HasMaxLength(50)
                .HasColumnName("nvSysTableName");
        });

        modelBuilder.Entity<TsysTableRow>(entity =>
        {
            entity.HasKey(e => e.IIdSysTableRow).HasName("PK__TSysTabl__630A2C6C0AD2A005");

            entity.ToTable("TSysTableRow");

            entity.Property(e => e.IIdSysTableRow).HasColumnName("iIdSysTableRow");
            entity.Property(e => e.IIdSysTable).HasColumnName("iIdSysTable");
            entity.Property(e => e.NvValueEng)
                .HasMaxLength(100)
                .HasColumnName("nvValueEng");
            entity.Property(e => e.NvValueHeb)
                .HasMaxLength(100)
                .HasColumnName("nvValueHeb");

            entity.HasOne(d => d.IIdSysTableNavigation).WithMany(p => p.TsysTableRows)
                .HasForeignKey(d => d.IIdSysTable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TSysTable__iIdSy__0CBAE877");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}