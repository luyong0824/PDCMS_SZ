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
    /// WorkOrderDetail表配置
    /// </summary>
    internal class WorkOrderDetailConfiguration : EntityTypeConfiguration<WorkOrderDetail>
    {
        public WorkOrderDetailConfiguration()
        {
            ToTable("tbl_WorkOrderDetail");
            HasKey<Guid>(e => e.Id);
            Property(e => e.WorkOrderId)
                .IsRequired();
            Property(e => e.WorkBeginDate)
                .IsRequired();
            Property(e => e.BeginHour)
                .IsRequired();
            Property(e => e.BeginMinute)
                .IsRequired();
            Property(e => e.WorkEndDate)
                .IsRequired();
            Property(e => e.EndHour)
                .IsRequired();
            Property(e => e.EndMinute)
                .IsRequired();
            Property(e => e.IsFinish)
                .IsRequired();
            Property(e => e.ExecuteSituation)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.MaterialConsumption)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.PersonnelNumber)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.CarType)
                .IsRequired()
                .HasMaxLength(500);
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
