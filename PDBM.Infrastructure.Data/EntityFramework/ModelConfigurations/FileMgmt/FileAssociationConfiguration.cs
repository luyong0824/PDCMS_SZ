using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.FileMgmt;

namespace PDBM.Infrastructure.Data.EntityFramework.ModelConfigurations.FileMgmt
{
    /// <summary>
    /// FileAssociation表配置
    /// </summary>
    internal class FileAssociationConfiguration : EntityTypeConfiguration<FileAssociation>
    {
        public FileAssociationConfiguration()
        {
            ToTable("tbl_FileAssociation");
            HasKey<Guid>(e => e.Id);
            Property(e => e.EntityName)
                .IsRequired()
                .HasMaxLength(50);
            Property(e => e.EntityId)
                .IsRequired();
            Property(e => e.FileIdList)
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
