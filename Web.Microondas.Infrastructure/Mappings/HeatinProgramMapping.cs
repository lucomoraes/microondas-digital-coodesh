using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Microondas.Domain.Entities;

namespace Web.Microondas.Infrastructure.Mappings;

internal class HeatinProgramMapping : IEntityTypeConfiguration<HeatingProgram>
{
    public void Configure(EntityTypeBuilder<HeatingProgram> builder)
    {
        builder.ToTable("HeatingPrograms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uniqueidentifier") 
            .IsRequired();


        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasColumnType("nvarchar(100)")
            .IsRequired();


        builder.Property(x => x.Food)
            .HasColumnName("food")
            .HasColumnType("nvarchar(200)")
            .IsRequired();


        builder.Property(x => x.TimeInSeconds)
            .HasColumnName("time_in_seconds")
            .HasColumnType("int")
            .IsRequired();


        builder.Property(x => x.Power)
            .HasColumnName("power")
            .HasColumnType("int")
            .IsRequired();


        builder.Property(x => x.Character)
            .HasColumnName("character")
            .HasColumnType("nchar(1)")
            .IsRequired()
            .IsFixedLength();


        builder.Property(x => x.Instructions)
            .HasColumnName("instructions")
            .HasColumnType("nvarchar(500)");


        builder.Property(x => x.IsPreset)
            .HasColumnName("is_preset")
            .HasColumnType("bit")
            .IsRequired();


        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetime2")
            .IsRequired();

        builder.HasIndex(x => x.Character)
            .IsUnique();
    }
}