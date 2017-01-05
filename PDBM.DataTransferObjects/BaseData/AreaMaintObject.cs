using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BaseData
{
    /// <summary>
    /// 区域维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class AreaMaintObject
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 区域编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string AreaCode
        {
            get;
            set;
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 参考经度
        /// </summary>
        [DataMember, ProtoMember(4)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 参考纬度
        /// </summary>
        [DataMember, ProtoMember(5)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid? AreaManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理名字
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string AreaManagerFullName
        {
            get;
            set;
        }
    }
}
