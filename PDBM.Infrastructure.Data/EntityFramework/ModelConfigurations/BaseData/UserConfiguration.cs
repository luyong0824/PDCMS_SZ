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
    /// User表配置
    /// </summary>
    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("tbl_User");
            HasKey<Guid>(e => e.Id);
            Property(e => e.DepartmentId)
                .IsRequired();
            Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.UserPassword)
                .IsRequired()
                .IsUnicode(false);
            Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.State)
                .IsRequired();
            Property(e => e.IsCurrentUsed)
                .IsRequired();
            Property(e => e.UniqueCode)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.ModifyUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
            Property(e => e.ModifyDate)
                .IsRequired();
            HasMany(e => e.ManagerUserDepartments).WithOptional(e => e.ManagerUser).HasForeignKey(e => e.ManagerUserId);
            HasMany(e => e.ManagerUserProjects).WithRequired(e => e.ManagerUser).HasForeignKey(e => e.ManagerUserId);
            HasMany(e => e.ResponsibleUserProjects).WithRequired(e => e.ResponsibleUser).HasForeignKey(e => e.ResponsibleUserId);
            HasMany(e => e.AddressingUserPlannings).WithOptional(e => e.AddressingUser).HasForeignKey(e => e.AddressingUserId);
        }
    }
}
