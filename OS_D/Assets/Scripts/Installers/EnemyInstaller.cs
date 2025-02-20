
using Zenject;
using Game.Enemy;

namespace Game
{
    public sealed class EnemyInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            EnemyBase enemy = gameObject.GetComponent<EnemyBase>();

            Container
                .Bind<EnemyBase>()
                .FromInstance(enemy)
                .AsSingle();

            Container
                .BindInterfacesTo<AIService>()
                .AsSingle();
        }
    }

}