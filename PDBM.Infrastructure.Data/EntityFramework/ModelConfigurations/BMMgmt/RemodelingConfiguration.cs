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
    /// Remodeling表配置
    /// </summary>
    internal class RemodelingConfiguration : EntityTypeConfiguration<Remodeling>
    {
        public RemodelingConfiguration()
        {
            ToTable("tbl_Remodeling");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.PlaceCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PlaceId)
                .IsRequired();
            Property(e => e.ProposedNetwork)
                .IsRequired()
                .HasMaxLength(250);
            Property(e => e.OrderState)
                .IsRequired();
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(250);
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
