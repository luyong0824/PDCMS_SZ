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
    /// File表配置
    /// </summary>
    internal class FileConfiguration : EntityTypeConfiguration<File>
    {
        public FileConfiguration()
        {
            ToTable("tbl_File");
            HasKey<Guid>(e => e.Id);
            Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);
            Property(e => e.FileType)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
            Property(e => e.FileExtension)
                .IsRequired()
                .HasMaxLength(255);
            Property(e => e.FileSize)
                .IsRequired();
            Property(e => e.FilePath)
                .IsRequired()
                .HasMaxLength(400);
            Property(e => e.UploadUserId)
                .IsRequired();
            Property(e => e.UploadDate)
                .IsRequired();
        }
    }
}
