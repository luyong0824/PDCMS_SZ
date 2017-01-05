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
    /// MaterialList表配置
    /// </summary>
    internal class MaterialListConfiguration : EntityTypeConfiguration<MaterialList>
    {
        public MaterialListConfiguration()
        {
            ToTable("tbl_MaterialList");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ParentId)
                .IsRequired();
            Property(e => e.PropertyType)
                .IsRequired();
            Property(e => e.MaterialId)
                .IsRequired();
            Property(e => e.MaterialSpecId)
                .IsOptional();
            Property(e => e.BudgetPrice)
                .IsRequired()
                .HasPrecision(18, 2);
            Property(e => e.SpecNumber)
                .IsRequired()
                .HasPrecision(18, 2);
            Property(e => e.SupplierId)
                .IsOptional();
            Property(e => e.Memos)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.DoState)
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
