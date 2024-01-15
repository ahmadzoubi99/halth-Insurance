using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<About> Abouts { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Home> Homes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Useraccount> Useraccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=xe)));User Id=C##MVC123;Password=test123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##MVC123")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<About>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008562");

            entity.ToTable("ABOUT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Image1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGE1");
            entity.Property(e => e.Image2)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGE2");
            entity.Property(e => e.Image3)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGE3");
            entity.Property(e => e.Text1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT1");
            entity.Property(e => e.Text2)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT2");
            entity.Property(e => e.Text3)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT3");
            entity.Property(e => e.Text4)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT4");
            entity.Property(e => e.Text5)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT5");
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008550");

            entity.ToTable("BANK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER")
                .HasColumnName("BALANCE");
            entity.Property(e => e.CardNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.Cvv)
                .HasColumnType("NUMBER")
                .HasColumnName("CVV");
            entity.Property(e => e.ExpirationDateMonth)
                .HasColumnType("NUMBER")
                .HasColumnName("EXPIRATION_DATE_MONTH");
            entity.Property(e => e.ExpirationDateYear)
                .HasColumnType("NUMBER")
                .HasColumnName("EXPIRATION_DATE_YEAR");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("OWNER_NAME");
        });

        modelBuilder.Entity<Beneficiary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008544");

            entity.ToTable("BENEFICIARIES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.BeneficiaryImageProof)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("BENEFICIARY_IMAGE_PROOF");
            entity.Property(e => e.BeneficiaryName)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("BENEFICIARY_NAME");
            entity.Property(e => e.BeneficiaryRelation)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("BENEFICIARY_RELATION");
            entity.Property(e => e.BeneficiaryState)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("BENEFICIARY_STATE");
            entity.Property(e => e.DateBeneficiary)
                .HasColumnType("DATE")
                .HasColumnName("DATE_BENEFICIARY");
            entity.Property(e => e.SubscriptionsId)
                .HasColumnType("NUMBER")
                .HasColumnName("SUBSCRIPTIONS_ID");

            entity.HasOne(d => d.Subscriptions).WithMany(p => p.Beneficiaries)
                .HasForeignKey(d => d.SubscriptionsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SUBSCRIPTIONS_ID");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008564");

            entity.ToTable("CONTACT_US");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.AddressText)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("ADDRESS_TEXT");
            entity.Property(e => e.Email)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.Text1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT1");
        });

        modelBuilder.Entity<Home>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008560");

            entity.ToTable("HOME");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.AboutUsTextFooter)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("ABOUT_US_TEXT_FOOTER");
            entity.Property(e => e.EmailContactUsFooter)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("EMAIL_CONTACT_US_FOOTER");
            entity.Property(e => e.Image1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGE1");
            entity.Property(e => e.Image2)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGE2");
            entity.Property(e => e.Image3)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGE3");
            entity.Property(e => e.LogoName)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("LOGO_NAME");
            entity.Property(e => e.PhoneNumberFooter)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER_FOOTER");
            entity.Property(e => e.Text1)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT1");
            entity.Property(e => e.Text2)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT2");
            entity.Property(e => e.Text3)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT3");
            entity.Property(e => e.Text4)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("TEXT4");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008537");

            entity.ToTable("ROLES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008541");

            entity.ToTable("SUBSCRIPTIONS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.SubscriptionAmount)
                .HasColumnType("FLOAT")
                .HasColumnName("SUBSCRIPTION_AMOUNT");
            entity.Property(e => e.SubscriptionDate)
                .HasColumnType("DATE")
                .HasColumnName("SUBSCRIPTION_DATE");
            entity.Property(e => e.SubscriptionDuration)
                .HasPrecision(3)
                .HasColumnName("SUBSCRIPTION_DURATION");
            entity.Property(e => e.SubscriptionsStatus)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("SUBSCRIPTIONS_STATUS");
            entity.Property(e => e.UserAccountId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ACCOUNT_ID");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USER_ID");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008547");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Status)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.TestimonialDate)
                .HasColumnType("DATE")
                .HasColumnName("TESTIMONIAL_DATE");
            entity.Property(e => e.TestimonialText)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIAL_TEXT");
            entity.Property(e => e.UseraccountId)
                .HasColumnType("NUMBER")
                .HasColumnName("USERACCOUNT_ID");

            entity.HasOne(d => d.Useraccount).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UseraccountId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_USERSS_ID");
        });

        modelBuilder.Entity<Useraccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008539");

            entity.ToTable("USERACCOUNT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Email)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FullName)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("FULL_NAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Passwd)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("PASSWD");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.RolesId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLES_ID");
            entity.Property(e => e.Username)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Roles).WithMany(p => p.Useraccounts)
                .HasForeignKey(d => d.RolesId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_ROLES_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
