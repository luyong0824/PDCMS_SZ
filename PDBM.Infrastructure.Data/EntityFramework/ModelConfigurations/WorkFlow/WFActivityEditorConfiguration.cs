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
    /// WFActivityEditor表配置
    /// </summary>
    internal class WFActivityEditorConfiguration : EntityTypeConfiguration<WFActivityEditor>
    {
        public WFActivityEditorConfiguration()
        {
            ToTable("tbl_WFActivityEditor");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WFCategoryId)
                .IsRequired();
            Property(e => e.WFActivityEditorCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.WFActivityEditorName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.EditorUrl)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(200);
            Property(e => e.IsMustEdit)
                .IsRequired();
            Property(e => e.State)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
