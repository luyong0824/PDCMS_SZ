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
    /// Department表配置
    /// </summary>
    internal class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            ToTable("tbl_Department");
            HasKey<Guid>(e => e.Id);
            Property(e => e.CompanyId)
                .IsRequired();
            Property(e => e.DepartmentCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.DepartmentName)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.ManagerUserId)
                .IsOptional();
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(150);
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
