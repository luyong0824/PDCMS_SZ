using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.WorkFlow
{
    public class WFActivityInstanceEditor : AggregateRoot
    {
        protected WFActivityInstanceEditor()
        { 
        }

        internal WFActivityInstanceEditor(Guid wfActivityInstanceId)
        {
            wfActivityInstanceId.IsEmpty("流程步骤Id");

            this.Id = Guid.NewGuid();
            this.WFActivityInstanceId = wfActivityInstanceId;
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 流程步骤Id
        /// </summary>
        public Guid WFActivityInstanceId
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
    }
}
