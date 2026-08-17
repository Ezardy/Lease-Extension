namespace Aniki.Common {
	public interface ITyped<T> where T : class {
		public static bool	IsInstance(object instance) {
			return instance is ITyped<T>;
		}
	}
}
