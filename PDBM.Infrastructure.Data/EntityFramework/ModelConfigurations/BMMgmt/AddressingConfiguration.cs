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
    /// Addressing表配置
    /// </summary>
    internal class AddressingConfiguration : EntityTypeConfiguration<Addressing>
    {
        public AddressingConfiguration()
        {
            ToTable("tbl_Addressing");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.AddressingDate)
                .IsRequired();
            Property(e => e.PlanningId)
                .IsRequired();
            Property(e => e.PlaceName)
                .IsRequired()
                .HasMaxLength(100);
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
            Property(e => e.AreaManagerId)
                .IsRequired();
            Property(e => e.DesignCustomerId)
                .IsRequired();
            Property(e => e.OrderState)
                .IsRequired();
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
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
