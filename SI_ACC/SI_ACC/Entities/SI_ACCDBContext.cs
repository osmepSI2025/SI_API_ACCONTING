using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SI_ACC.Entities;

public partial class SI_ACCDBContext : DbContext
{
    public SI_ACCDBContext()
    {
    }

    public SI_ACCDBContext(DbContextOptions<SI_ACCDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MApiInformation> MApiInformations { get; set; }

    public virtual DbSet<MBudgetavailable> MBudgetavailables { get; set; }

    public virtual DbSet<MBudgetentire> MBudgetentires { get; set; }

    public virtual DbSet<MScheduledJob> MScheduledJobs { get; set; }

    public virtual DbSet<TBudgetMapping> TBudgetMappings { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=192.168.9.155;Database=bluecarg_SME_API_Account;User Id=sa;Password=Osmep@2025;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Thai_CI_AS");

        modelBuilder.Entity<MApiInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MApiInformation");

            entity.ToTable("M_ApiInformation", "SME_Account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApiKey).HasMaxLength(150);
            entity.Property(e => e.AuthorizationType).HasMaxLength(50);
            entity.Property(e => e.Bearer).HasColumnType("ntext");
            entity.Property(e => e.ContentType).HasMaxLength(150);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MethodType).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(150);
            entity.Property(e => e.ServiceNameCode).HasMaxLength(250);
            entity.Property(e => e.ServiceNameTh).HasMaxLength(250);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Urldevelopment).HasColumnName("URLDevelopment");
            entity.Property(e => e.Urlproduction).HasColumnName("URLProduction");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<MBudgetavailable>(entity =>
        {
            entity.ToTable("M_Budgetavailable", "SME_Account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AdvanceAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AllocateAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AvailableAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BudgetAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BudgetExpireDate).HasColumnType("date");
            entity.Property(e => e.BudgetId).HasMaxLength(50);
            entity.Property(e => e.BudgetLv1Code).HasMaxLength(50);
            entity.Property(e => e.BudgetLv2Code).HasMaxLength(50);
            entity.Property(e => e.BudgetLv3Code).HasMaxLength(50);
            entity.Property(e => e.BudgetLv4Code).HasMaxLength(50);
            entity.Property(e => e.BudgetLv5Code).HasMaxLength(50);
            entity.Property(e => e.BudgetLv6Code).HasMaxLength(50);
            entity.Property(e => e.BudgetName).HasMaxLength(255);
            entity.Property(e => e.BudgetReceive).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BudgetReturn).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BudgetStartDate).HasColumnType("date");
            entity.Property(e => e.BudgetStatus).HasMaxLength(50);
            entity.Property(e => e.DetailInitialAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InitialAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OutsourceFund).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReferenceBudgetCode).HasMaxLength(100);
            entity.Property(e => e.ReservedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VoucherPaymentAmt).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<MBudgetentire>(entity =>
        {
            entity.ToTable("M_Budgetentires", "SME_Account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AdvanceAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ApiMappingId).HasMaxLength(100);
            entity.Property(e => e.AuxiliaryIndex2).HasMaxLength(100);
            entity.Property(e => e.BudgetActivites).HasMaxLength(255);
            entity.Property(e => e.BudgetCode).HasMaxLength(100);
            entity.Property(e => e.BudgetDepartment).HasMaxLength(100);
            entity.Property(e => e.BudgetName).HasMaxLength(255);
            entity.Property(e => e.BudgetPlan).HasMaxLength(100);
            entity.Property(e => e.BudgetProject).HasMaxLength(100);
            entity.Property(e => e.BudgetStrategies).HasMaxLength(255);
            entity.Property(e => e.BudgetYear).HasMaxLength(10);
            entity.Property(e => e.CurrentBudgetStatus).HasMaxLength(100);
            entity.Property(e => e.DocumentNo).HasMaxLength(100);
            entity.Property(e => e.OriginalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PostingDate).HasColumnType("date");
            entity.Property(e => e.ReferenceBudgetCode).HasMaxLength(100);
            entity.Property(e => e.RemainingAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReservedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransDescription).HasMaxLength(500);
            entity.Property(e => e.TransDescription2).HasMaxLength(500);
            entity.Property(e => e.TransferToCode).HasMaxLength(100);
            entity.Property(e => e.VoucherAdvAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VoucherPaymentAmt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VoucherReceiveAmt).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<MScheduledJob>(entity =>
        {
            entity.ToTable("M_ScheduledJobs", "SME_Account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JobName).HasMaxLength(150);
        });

        modelBuilder.Entity<TBudgetMapping>(entity =>
        {
            entity.ToTable("T_BudgetMapping", "SME_Account");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BudgetId).HasMaxLength(50);
            entity.Property(e => e.BudgetName).HasMaxLength(255);
            entity.Property(e => e.MappingCode).HasMaxLength(50);
            entity.Property(e => e.MappingName).HasMaxLength(255);
            entity.Property(e => e.MappingParentCode).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
