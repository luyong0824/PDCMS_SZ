using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BaseData;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BaseData
{
    /// <summary>
    /// Place表配置
    /// </summary>
    internal class PlaceConfiguration : EntityTypeConfiguration<Place>
    {
        public PlaceConfiguration()
        {
            ToTable("tbl_Place");
            HasKey<Guid>(e => e.Id);
            Property(e => e.PlaceCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PlaceName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.PlaceCategoryId)
                .IsRequired();
            Property(e => e.ReseauId)
                .IsRequired();
            Property(e => e.Lng)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.Lat)
                .IsRequired()
                .HasPrecision(18, 5);
            Property(e => e.PlaceOwner)
                .IsRequired();
            Property(e => e.Importance)
                .IsRequired();
            Property(e => e.AddressingDepartmentId)
                .IsRequired();
            Property(e => e.AddressingRealName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.OwnerName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.OwnerContact)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.OwnerPhoneNumber)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.DetailedAddress)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.State)
                .IsRequired();
            Property(e => e.G2Number)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.D2Number)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.G3Number)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.G4Number)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.G5Number)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.PlaceMapState)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.ModifyUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
            Property(e => e.ModifyDate)
                .IsRequired();
        }
    }
}
