using PDBM.Domain.Models.BaseData;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BaseData
{
    /// <summary>
    /// Material表配置
    /// </summary>
    internal class MaterialConfiguration : EntityTypeConfiguration<Material>
    {
        public MaterialConfiguration()
        {
            ToTable("tbl_Material");
            HasKey<Guid>(e => e.Id);
            Property(e => e.MaterialCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.MaterialName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.MaterialCategoryId)
                .IsRequired();
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
