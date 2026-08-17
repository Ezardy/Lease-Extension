using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Aniki.Common {
	public static class ZenjectExtensions {
		public static CopyNonLazyBinder	WithType<T>(this ConcreteIdArgConditionCopyNonLazyBinder binder) where T : class {
			return binder.When(c => OfType<T>(c.ObjectType));
		}

		public static CopyNonLazyBinder	WithTypeOrAny<T>(this ConcreteIdArgConditionCopyNonLazyBinder binder) where T : class {
			return binder.When(c => 
				OfType<T>(c.ObjectType)
				|| (c.MemberType.IsGenericType
					&& c.MemberType.GetGenericTypeDefinition() == typeof(List<>)));
		}

		private static bool	OfType<T>(Type type) where T : class {
			return type.GetInterfaces().Any(i =>
				i.IsGenericType
				&& i.GetGenericTypeDefinition() == typeof(ITyped<>)
				&& i.GetGenericArguments()[0] == typeof(T)
			);
		}
	}
}
