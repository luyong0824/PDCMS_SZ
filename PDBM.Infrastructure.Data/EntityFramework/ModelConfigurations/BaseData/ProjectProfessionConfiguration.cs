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
    /// 项目专业表配置
    /// </summary>
    internal class ProjectProfessionConfiguration : EntityTypeConfiguration<ProjectProfession>
    {
        public ProjectProfessionConfiguration()
        {
            ToTable("tbl_ProjectProfession");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ProjectId)
                .IsRequired();
            Property(e => e.Profession)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
