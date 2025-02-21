
using UnityEngine;
using Zenject;

namespace Game
{
    [CreateAssetMenu(
        fileName ="GameSystemInstaller",
        menuName ="Installers/New GameSystemInstaller")]
    public sealed class GameSystemInstaller : ScriptableObjectInstaller
    {
        private readonly Vector3 cameraOffset = new Vector3(0, 0, 10);
        public override void InstallBindings()
        {
          
            Container
                .BindInterfacesTo<MoveInput>()
                .AsSingle();

            Container
                .BindInterfacesTo<MoveController>() //TODO: Разобраться почему спавнится без NonLazy
                .AsSingle(); // Заменить на AsCached если будут проблемы
                //.NonLazy();


            Container
                .Bind<IEnemyFactory>()
                .To<EnemyFactory>()
                .AsSingle();

            Container
                .BindInterfacesTo<CameraFollower>()
                .AsSingle()
                .WithArguments(cameraOffset);

            Container
                .Bind<GameCycle>()
                .AsSingle();

            Container
                .BindInterfacesTo<GameCycleController>()
                .AsSingle();

        }
    }
}