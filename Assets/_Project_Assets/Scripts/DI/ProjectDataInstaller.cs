using Scripts.Data.ScriptableObject;
using UnityEngine;
using Zenject;

namespace Scripts.DI
{
    [CreateAssetMenu(fileName = "ProjectDataInstaller", menuName = "Data/ProjectDataInstaller")]
    public class ProjectDataInstaller : ScriptableObjectInstaller<ProjectDataInstaller>
    {
        [SerializeField] private DifficultyLevelData difficultyLevelData;

        public override void InstallBindings()
        {
            Container.BindInstance(difficultyLevelData);
        }
    }
}