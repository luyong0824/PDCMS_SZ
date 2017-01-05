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
    /// MenuItem表配置
    /// </summary>
    internal class MenuItemConfiguration : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemConfiguration()
        {
            ToTable("tbl_MenuItem");
            HasKey<Guid>(e => e.Id);
            Property(e => e.MenuSubId)
                .IsRequired();
            Property(e => e.MenuItemName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.MenuItemPath)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);
            Property(e => e.IndexId)
                .IsRequired();
            Property(e => e.IsDisplay)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
