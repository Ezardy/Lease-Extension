using Aniki.Character;
using Aniki.UI;
using Aniki.SceneManagment;
using Aniki.World;
using Aniki.Input;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Aniki.Common {
	[CreateAssetMenu(fileName = "CommonInstaller", menuName = "Installers/Common Installer")]
	internal class CommonInstaller : ScriptableObjectInstaller<CommonInstaller> {
		[SerializeField] private LayerNames	layerNames;

		public override void InstallBindings() {
			InstallMessagePipe();
			Container.BindInstance(layerNames);
		}

		private void	InstallMessagePipe() {
			MessagePipeOptions	options = Container.BindMessagePipe();
	
			Container.BindMessageBroker<CharacterState>(options);
			Container.BindMessageBroker<AcceptSentenceMessage>(options);
			Container.BindMessageBroker<FocusedScene>(options);

			InstallInput(options);
			InstallCollisions(options);
	
			Container.BindFactory<MessagePipeDiagnostics, MessagePipeDiagnostics.Factory>().FromFactory<MessagePipeDiagnosticsFactory>();
			Container.BindInterfacesTo<MessagePipeDiagnosticsBootstrap>().AsSingle();
		}

		private void	InstallCollisions(MessagePipeOptions options) {
			Container.BindMessageBroker<ObstacleCollisionMessage>(options);
			Container.BindMessageBroker<FloorCollisionMessage>(options);
			Container.BindMessageBroker<ToothCollisionMessage>(options);
			Container.BindMessageBroker<BarPassedMessage>(options);
		}

		private void	InstallInput(MessagePipeOptions options) {
			Container.BindMessageBroker<PunchInputMessage>(options);
			Container.BindMessageBroker<TapInputMessage>(options);
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