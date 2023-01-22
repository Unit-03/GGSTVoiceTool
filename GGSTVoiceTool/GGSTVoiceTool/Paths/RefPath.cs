using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GGSTVoiceTool
{
	public class RefPath
	{
		#region Regex

		private static Regex BackReferenceMatch = new(@"(?<full>\@(?<backref>.*?)\@)", RegexOptions.Compiled);

		#endregion

		#region Fields

		private Type startType;
		private string currentPath;

		private Dictionary<string, PropertyInfo> backReferences;

		#endregion

		#region Constructors

		public RefPath(string path, Type context)
		{
			startType = context;
			currentPath = path;

			GetBackReferences();
		}

		#endregion

		#region Methods

		private void GetBackReferences()
		{
			backReferences = new();

			MatchCollection matches = BackReferenceMatch.Matches(currentPath);

			foreach (Match match in matches)
			{
				if (match.Success && match.Groups.TryGetValue("backref", out Group backref))
				{
					FieldInfo infoF = startType.GetField(backref.Value).is


					PropertyInfo info = startType.GetProperty(backref.Value, typeof(RefPath));

					if (info == null)
					{
						string[] parts = backref.Value.Split('.');
						Type root = typeof(Paths);

						for (int i = 0; i < parts.Length - 1; ++i)
						{
							Type child = root.GetNestedType(parts[i]);

							if (child == null)
							{
								root = null;
								break;
							}

							root = child;
						}

						info = root?.GetProperty(parts[^1]);

						bool finalised = false;
						Type backrefType = info.PropertyType;

						if (backrefType == typeof(RefPath))
						{
							object value = info.GetValue(null);

							if (value is RefPath path && path.IsFinalised() == true)
								finalised = true;
						}
					}

					if (info != null)
						backReferences.Add(match.Groups["full"].Value, info);
				}
			}
		}

		public bool? IsFinalised()
		{
			if (backReferences.Count == 0)
				return true;


		}

		public string Evaluate()
		{
			if (backReferences.Count == 0)
				return currentPath;

			string evaluated = currentPath;

			if (backReferences.Count > 0)
			{
				string[] keys = new string[backReferences.Count];
				int index = 0;

				foreach (string key in backReferences.Keys)
					keys[index++] = key;

				for (int i = 0; i < keys.Length; ++i)
				{
					object value = backReferences[keys[i]].GetValue(null);

					if (value == null)
						return null;

					string valueStr = value.ToString();
					evaluated = evaluated.Replace(keys[i], valueStr);

					if (value is RefPath path)
					{
						// If we're evaluating a backreference with no properties then there's no need to continue evaluating it in the future
						//  We can just substitute it into the path and remove the backreference
						if (path.HasProperties() == false)
						{
							currentPath = currentPath.Replace(keys[i], valueStr);
							backReferences.Remove(keys[i]);
						}
					}
				}
			}

			if (properties.Count > 0)
			{
				foreach (var property in properties)
				{
					object value = property.Value.GetValue(null);
					evaluated = evaluated.Replace(property.Key, value?.ToString());
				}
			}

			return evaluated;
		}

		public override string ToString()
		{
			return Evaluate();
		}

		#endregion

		#region Operators

		public static implicit operator string(RefPath path) => path.Evaluate();

		#endregion
	}
}
