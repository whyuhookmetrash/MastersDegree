using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class SceneInstaller: MonoInstaller
    {
        public Transform playerSpawnPoint;
        public GameObject playerPrefab;
        public override void InstallBindings()
        {

            Container
                .BindInterfacesTo<SceneInstaller>()
                .FromInstance(this)
                .AsSingle();

            Player player = Container
                .InstantiatePrefabForComponent<Player>(playerPrefab, Vector3.zero, Quaternion.identity, null); //TODO передать извне позицию спавна?

            Container
                .Bind<Player>()
                .FromInstance(player)
                .AsSingle();

            Container
                .Bind<Camera>()
                .FromComponentInHierarchy()
                .AsSingle();

        }

    }
}