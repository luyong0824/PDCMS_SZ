using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 项目立项信息表维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ProjectCodeListMaintObject
    {
        /// <summary>
        /// 项目立项表Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 立项编号
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 建设方式
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int ProjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 立项时间
        /// </summary>
        [DataMember, ProtoMember(4)]
        public DateTime ProjectDate
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ProjectManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 使用状态
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(11)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(12)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户名称
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string CreateFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理用户名称
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string ProjectManagerFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格名称
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 立项时间文本
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string ProjectDateText
        {
            get;
            set;
        }
    }
}
