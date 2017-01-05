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
    /// PostUser表配置
    /// </summary>
    internal class PostUserConfiguration : EntityTypeConfiguration<PostUser>
    {
        public PostUserConfiguration()
        {
            ToTable("tbl_PostUser");
            HasKey<Guid>(e => e.Id);
            Property(e => e.PostId)
                .IsRequired();
            Property(e => e.UserId)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
