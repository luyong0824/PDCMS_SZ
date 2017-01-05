using PDBM.Domain.Models.WorkFlow;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.WorkFlow
{
    internal class WFActivityInstanceEditorConfiguration : EntityTypeConfiguration<WFActivityInstanceEditor>
    {
        public WFActivityInstanceEditorConfiguration()
        {
            ToTable("tbl_WFActivityInstanceEditor");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WFActivityInstanceId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
