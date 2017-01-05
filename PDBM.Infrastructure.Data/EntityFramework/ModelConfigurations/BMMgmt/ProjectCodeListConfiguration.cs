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
    /// ProjectCodeList表配置
    /// </summary>
    internal class ProjectCodeListConfiguration : EntityTypeConfiguration<ProjectCodeList>
    {
        public ProjectCodeListConfiguration()
        {
            ToTable("tbl_ProjectCodeList");
            HasKey<Guid>(e => e.Id);
            Property(e => e.ProjectCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ProjectType)
                .IsRequired();
            Property(e => e.ProjectDate)
                .IsRequired();
            Property(e => e.PlaceName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.ReseauId)
                .IsRequired();
            Property(e => e.ProjectManagerId)
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
