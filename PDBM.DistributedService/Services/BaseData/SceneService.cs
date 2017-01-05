using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 周边场景分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SceneService : ISceneService
    {
        private readonly ISceneService sceneServiceImpl = ServiceLocator.Instance.GetService<ISceneService>();

        public SceneMaintObject GetSceneById(Guid id)
        {
            try
            {
                return sceneServiceImpl.GetSceneById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<SceneMaintObject> GetScenes()
        {
            try
            {
                return sceneServiceImpl.GetScenes();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<SceneSelectObject> GetUsedScenes()
        {
            try
            {
                return sceneServiceImpl.GetUsedScenes();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateScene(SceneMaintObject sceneMaintObject)
        {
            try
            {
                sceneServiceImpl.AddOrUpdateScene(sceneMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveScenes(IList<SceneMaintObject> sceneMaintObjects)
        {
            try
            {
                sceneServiceImpl.RemoveScenes(sceneMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            sceneServiceImpl.Dispose();
        }
    }
}
