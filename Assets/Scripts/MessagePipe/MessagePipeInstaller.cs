using Aniki.Character;
using Aniki.UI;
using Aniki.SceneManagment;
using LeaseExtension;
using LeaseExtension.Gameplay.Contract.Message;
using LeaseExtension.Input.Contract;
using LeaseExtension.World.Entities.Message;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.Common {
	[CreateAssetMenu(fileName = "MessagePipeInstaller", menuName = "Installers/Message Pipe Installer")]
	internal class MessagePipeInstaller : ScriptableObjectInstaller<MessagePipeInstaller> {
		public override void InstallBindings() {
			InstallMessagePipe();
		}

		private void	InstallMessagePipe() {
			MessagePipeOptions	options = Container.BindMessagePipe();
	
			Container.BindMessageBroker<CharacterState>(options);
			Container.BindMessageBroker<RestartRequested>(options);
			Container.BindMessageBroker<FocusedScene>(options);

			InstallInput(options);
			InstallCollisions(options);
	
			Container.BindFactory<MessagePipeDiagnostics, MessagePipeDiagnostics.Factory>().FromFactory<MessagePipeDiagnosticsFactory>();
			Container.BindInterfacesTo<MessagePipeDiagnosticsBootstrap>().AsSingle();
		}
		
		private void	InstallCollisions(MessagePipeOptions options) {
			Container.BindMessageBroker<ObstacleCollided>(options);
			Container.BindMessageBroker<FloorCollided>(options);
			Container.BindMessageBroker<ToothPicked>(options);
			Container.BindMessageBroker<BarPassed>(options);
		}

		private void	InstallInput(MessagePipeOptions options) {
			Container.BindMessageBroker<PunchRequested>(options);
			Container.BindMessageBroker<Tapped>(options);
		}

		private class MessagePipeDiagnosticsBootstrap : IInitializable {
			private readonly MessagePipeDiagnostics.Factory	factory;

			public void	Initialize() {
				factory.Create();
			}

			public MessagePipeDiagnosticsBootstrap(MessagePipeDiagnostics.Factory factory) {
				this.factory = factory;
			}
		}

		private class MessagePipeDiagnostics {
			public class Factory : PlaceholderFactory<MessagePipeDiagnostics> { }
		}
	
		private class MessagePipeDiagnosticsFactory : IFactory<MessagePipeDiagnostics> {
			private readonly DiContainer	container;
	
			public MessagePipeDiagnosticsFactory(DiContainer container) {
				this.container = container;
			}

			public MessagePipeDiagnostics	Create() {
				GlobalMessagePipe.SetProvider(container.AsServiceProvider());
				return new();
			}
		}
	}
}