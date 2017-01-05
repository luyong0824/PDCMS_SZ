using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 周边场景实体
    /// </summary>
    public class Scene : AggregateRoot
    {
        protected Scene()
        {
        }

        /// <summary>
        /// 构造周边场景实体
        /// </summary>
        /// <param name="sceneCode">周边场景编码</param>
        /// <param name="sceneName">周边场景名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">周边场景状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Scene(string sceneCode, string sceneName, string remarks, State state, Guid createUserId)
        {
            sceneCode.IsNullOrEmptyOrTooLong("周边场景编码", true, 50);
            sceneName.IsNullOrEmptyOrTooLong("周边场景名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("周边场景状态");

            this.Id = Guid.NewGuid();
            this.SceneCode = sceneCode;
            this.SceneName = sceneName;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 周边场景编码
        /// </summary>
        public string SceneCode
        {
            get;
            set;
        }

        /// <summary>
        /// 周边场景名称
        /// </summary>
        public string SceneName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 周边场景状态
        /// </summary>
        public State State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyDate
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 站点实体列表
        /// </summary>
        protected virtual ICollection<Place> Places
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址确认实体列表
        /// </summary>
        protected virtual ICollection<Addressing> Addressings
        {
            get;
            set;
        }

        /// <summary>
        /// 购置站点实体列表
        /// </summary>
        protected virtual ICollection<Purchase> Purchases
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改周边场景实体
        /// </summary>
        /// <param name="sceneCode">周边场景编码</param>
        /// <param name="sceneName">周边场景名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">周边场景状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string sceneCode, string sceneName, string remarks, State state, Guid modifyUserId)
        {
            sceneCode.IsNullOrEmptyOrTooLong("周边场景编码", true, 50);
            sceneName.IsNullOrEmptyOrTooLong("周边场景名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("周边场景状态");

            this.SceneCode = sceneCode;
            this.SceneName = sceneName;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
