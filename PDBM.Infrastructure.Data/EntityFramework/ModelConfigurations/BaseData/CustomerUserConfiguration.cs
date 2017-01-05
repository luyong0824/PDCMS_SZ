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
    /// CustomerUser表配置
    /// </summary>
    internal class CustomerUserConfiguration : EntityTypeConfiguration<CustomerUser>
    {
        public CustomerUserConfiguration()
        {
            ToTable("tbl_CustomerUser");
            HasKey<Guid>(e => e.Id);
            Property(e => e.CustomerId)
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
