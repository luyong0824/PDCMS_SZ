using PDBM.Domain.Models.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    /// <summary>
    /// MaterialSpecList表配置
    /// </summary>
    internal class MaterialSpecListConfiguration : EntityTypeConfiguration<MaterialSpecList>
    {
        public MaterialSpecListConfiguration()
        {
            ToTable("tbl_MaterialSpecList");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ProjectCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.CustomerName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.MaterialSpecType)
                .IsRequired();
            Property(e => e.MaterialSpecName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.UnitPrice)
                .IsRequired();
            Property(e => e.SpecNumber)
                .IsRequired();
            Property(e => e.TotalPrice)
                .IsRequired();
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.State)
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
