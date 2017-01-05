using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.WorkFlow;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.WorkFlow
{
    /// <summary>
    /// WFCategory表配置
    /// </summary>
    internal class WFCategoryConfiguration : EntityTypeConfiguration<WFCategory>
    {
        public WFCategoryConfiguration()
        {
            ToTable("tbl_WFCategory");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WFCategoryCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.WFCategoryName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.EntityName)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            Property(e => e.CodePrefix)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PrintUrl)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(200);
            Property(e => e.WFActivityOperateList)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            Property(e => e.State)
                .IsRequired();
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
