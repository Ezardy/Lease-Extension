using LeaseExtension.Record;
using Zenject;

internal class RecordMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<RecordIncrementer>().AsSingle();
    }
}