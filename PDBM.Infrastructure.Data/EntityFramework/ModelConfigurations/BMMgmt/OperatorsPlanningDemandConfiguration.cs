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
    /// OperatorsPlanningDemand表配置
    /// </summary>
    internal class OperatorsPlanningDemandConfiguration : EntityTypeConfiguration<OperatorsPlanningDemand>
    {
        public OperatorsPlanningDemandConfiguration()
        {
            ToTable("tbl_OperatorsPlanningDemand");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OperatorsPlanningId)
                .IsRequired();
            Property(e => e.PlaceId)
                .IsRequired();
            Property(e => e.Demand)
                .IsRequired();
            Property(e => e.ConfirmUserId)
                .IsRequired();
            Property(e => e.ConfirmDate)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
