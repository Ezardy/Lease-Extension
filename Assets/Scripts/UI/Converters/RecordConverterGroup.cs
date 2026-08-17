using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aniki.UI {
	internal static class RecordConverterGroup {
		#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		#else
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		#endif
		private static void	RegisterConverters() {
			ConverterGroup	group = new("Record");

			group.AddConverter<uint, string>(BarsPassedToSentence);

			ConverterGroups.RegisterConverterGroup(group);
		}

		private static string	BarsPassedToSentence(ref uint barsPassed) {
			StringBuilder	sb = new(16);
			uint			days = barsPassed * 5;
			uint			months = days / 30;
			uint			years = months / 12;

			days %= 30;
			months %= 12;
			if (years > 0) {
				sb.Append(years);
				sb.Append(" y");
			}
			if (months > 0) {
				if (years > 0)
					sb.Append(' ');
				sb.Append(months);
				sb.Append(" m");
			}
			if (days > 0 || (months == 0 && years == 0)) {
				if (months > 0 || years > 0)
					sb.Append(' ');
				sb.Append(days);
				sb.Append(" d");
			}
			return sb.ToString();
		}
	}
}
