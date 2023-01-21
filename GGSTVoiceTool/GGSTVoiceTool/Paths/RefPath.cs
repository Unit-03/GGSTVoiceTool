using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GGSTVoiceTool
{
	public class RefPath
	{
		#region Regex

		private static Regex BackreferenceMatch = new(@"(?<full>\@(?<backref>.*?)\@)", RegexOptions.Compiled);
		private static Regex PropertyMatch      = new(@"(?<full>\$\{(?<prop>.*?)\})",  RegexOptions.Compiled);
		
		private static Type PathsType      = typeof(Paths);
		private static Type PropertiesType = typeof(Paths.Properties);

		#endregion

		#region Fields

		private Type startType;
		private string currentPath;

		private Dictionary<string, PropertyInfo> backreferences;
		private Dictionary<string, PropertyInfo> properties;

		private bool? hasProperties;

		#endregion

		#region Constructors

		public RefPath(string path, Type context)
		{
			startType = context;
			currentPath = path;

			GetBackreferences();
			GetPropertyReferences();
		}

		#endregion

		#region Methods

		private void GetBackreferences()
		{
			backreferences = new();

			MatchCollection matches = BackreferenceMatch.Matches(currentPath);

			foreach (Match match in matches)
			{
				if (match.Success && match.Groups.TryGetValue("backref", out Group backref))
				{
					PropertyInfo info = startType.GetProperty(backref.Value, typeof(RefPath));

					if (info == null)
					{
						string[] parts = backref.Value.Split('.');
						Type root = PathsType;

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

						info = root?.GetProperty(parts[^1], typeof(RefPath));
					}

					if (info != null)
						backreferences.Add(match.Groups["full"].Value, info);
				}
			}
		}

		private void GetPropertyReferences()
		{
			properties = new();

			MatchCollection matches = PropertyMatch.Matches(currentPath);

			foreach (Match match in matches)
			{
				if (match.Success && match.Groups.TryGetValue("prop", out Group prop))
				{
					PropertyInfo info = PropertiesType.GetProperty(prop.Value);

					if (info != null)
						properties.Add(match.Groups["full"].Value, info);
				}
			}
		}

		private bool? HasProperties()
		{
			if (hasProperties != null)
				return hasProperties;

			if (properties.Count > 0)
				return hasProperties = true;

			foreach (PropertyInfo backref in backreferences.Values)
			{
				object value = backref.GetValue(null);

				// If the backreference value is null then we can't determine if it has properties or not
				//  return null to indicate that the current state is invalid
				if (value == null)
					return null;

				if (value is RefPath path)
				{ 
					if (path.HasProperties() == true)
						return hasProperties = true;
				}
			}

			return hasProperties = false;
		}

		public string Evaluate()
		{
			if (backreferences.Count == 0 && properties.Count == 0)
				return currentPath;

			string evaluated = currentPath;

			if (backreferences.Count > 0)
			{
				string[] keys = new string[backreferences.Count];
				int index = 0;

				foreach (string key in backreferences.Keys)
					keys[index++] = key;

				for (int i = 0; i < keys.Length; ++i)
				{
					object value = backreferences[keys[i]].GetValue(null);

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
							backreferences.Remove(keys[i]);
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
