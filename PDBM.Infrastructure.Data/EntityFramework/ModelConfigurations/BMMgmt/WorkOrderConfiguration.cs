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
    /// WorkOrder表配置
    /// </summary>
    internal class WorkOrderConfiguration : EntityTypeConfiguration<WorkOrder>
    {
        public WorkOrderConfiguration()
        {
            ToTable("tbl_WorkOrder");
            HasKey<Guid>(e => e.Id);
            Property(e => e.OrderCode)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.PlaceName)
                .IsOptional()
                .HasMaxLength(50);
            Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(150);
            Property(e => e.ReseauId)
                .IsRequired();
            Property(e => e.WorkSmallClassId)
                .IsRequired();
            Property(e => e.SceneContactMan)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.SceneContactTel)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.RequireSendDate)
                .IsRequired();
            Property(e => e.Days)
                .IsRequired();
            Property(e => e.CustomerId)
                .IsRequired();
            Property(e => e.CustomerUserId)
                .IsRequired();
            Property(e => e.WorkContent)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.HumanRequire)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.CarRequire)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.MaterialRequire)
                .IsRequired()
                .HasMaxLength(500);
            Property(e => e.Remarks)
                .IsRequired()
                .HasMaxLength(500);
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
            Property(e => e.RegisterUserId)
                .IsOptional();
            Property(e => e.RegisterDate)
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
            Property(e => e.OrderState)
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
