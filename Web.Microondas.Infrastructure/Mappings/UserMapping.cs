using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Microondas.Domain.Entities;

namespace Web.Microondas.Infrastructure.Mappings;

internal class UserMapping : IEntityTypeConfiguration<Users>
{
    public void Configure(EntityTypeBuilder<Users> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);


        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(x => x.Username)
            .HasColumnName("username")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(x => x.Firstname)
            .HasColumnName("firstname")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(x => x.Lastname)
            .HasColumnName("lastname")
            .HasColumnType("nvarchar(100)")
            .IsRequired();

        builder.Property(x => x.Password)
            .HasColumnName("password_hash")
            .HasColumnType("nvarchar(200)")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasIndex(x => x.Username)
            .IsUnique();

        builder.HasIndex(x => new { x.Username, x.Password });
    }
}
