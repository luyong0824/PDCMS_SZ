using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.BMMgmt
{
    /// <summary>
    /// OperatorsConfirmDetail表配置
    /// </summary>
    internal class OperatorsConfirmDetailConfiguration : EntityTypeConfiguration<OperatorsConfirmDetail>
    {
        public OperatorsConfirmDetailConfiguration()
        {
            ToTable("tbl_OperatorsConfirmDetail");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OperatorsConfirmId)
                .IsRequired();
            Property(e => e.PlanningId)
                .IsRequired();
            Property(e => e.TelecomDemand)
                .IsRequired();
            Property(e => e.MobileDemand)
                .IsRequired();
            Property(e => e.UnicomDemand)
                .IsRequired();
            Property(e => e.TelecomConfirmUserId)
                .IsRequired();
            Property(e => e.MobileConfirmUserId)
                .IsRequired();
            Property(e => e.UnicomConfirmUserId)
                .IsRequired();
            Property(e => e.TelecomConfirmDate)
                .IsRequired();
            Property(e => e.MobileConfirmDate)
                .IsRequired();
            Property(e => e.UnicomConfirmDate)
                .IsRequired();
            Property(e => e.CreateUserId)
                .IsRequired();
            Property(e => e.CreateDate)
                .IsRequired();
        }
    }
}
