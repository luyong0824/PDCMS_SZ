using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    /// <summary>
    /// Purchase表配置
    /// </summary>
    internal class PurchaseConfiguration : EntityTypeConfiguration<Purchase>
    {
        public PurchaseConfiguration()
        {
            ToTable("tbl_Purchase");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PurchaseDate)
                .IsRequired();
            Property(e => e.PlaceCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.GroupPlaceCode)
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
            Property(e => e.PropertyRight)
                .IsRequired();
            Property(e => e.Importance)
                .IsRequired();
            Property(e => e.SceneId)
                .IsRequired();
            Property(e => e.DetailedAddress)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.OwnerName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.OwnerContact)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.OwnerPhoneNumber)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.TelecomShare)
                .IsRequired();
            Property(e => e.MobileShare)
                .IsRequired();
            Property(e => e.UnicomShare)
                .IsRequired();
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.PlaceId)
                .IsRequired();
            Property(e => e.OrderState)
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
