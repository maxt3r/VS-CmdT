using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;


namespace Jitbit.CmdT
{
	class Utils
	{
		private DTE2 _applicationObject = null;
		///--------------------------------------------------------------------------------
		/// <summary>This property gets the visual studio IDE application object.</summary>
		///--------------------------------------------------------------------------------
		public DTE2 ApplicationObject
		{
			get
			{
				if (_applicationObject == null)
				{
					// Get an instance of the currently running Visual Studio IDE
					DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
					_applicationObject = dte as DTE2;
				}
				return _applicationObject;
			}
		}

		public static Solution2 Solution
		{
			get
			{
				DTE2 dte2 = Package.GetGlobalService(typeof (DTE)) as DTE2;
				return (Solution2) dte2.Solution;
			}
			private set { }
		}

		public IEnumerable<ProjectItem> Recurse(ProjectItems i)
		{
			if (i != null)
			{
				foreach (ProjectItem j in i)
				{
					foreach (ProjectItem k in Recurse(j))
					{
						yield return k;
					}
				}

			}
		}

		public IEnumerable<ProjectItem> Recurse(ProjectItem i)
		{
			yield return i;
			foreach (ProjectItem j in Recurse(i.ProjectItems))
			{
				yield return j;
			}
			if(i.SubProject != null)
			{
				foreach(ProjectItem k in Recurse(i.SubProject))
				{
					yield return k;
				}
			}
		}

		public IEnumerable<ProjectItem> Recurse(Project i)
		{
			foreach (ProjectItem j in Recurse(i.ProjectItems))
			{
				yield return j;
			}
		}

		public IEnumerable<ProjectItem> SolutionFiles()
		{
			DTE2 dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
			Solution2 soln = (Solution2)dte2.Solution;
			foreach (Project project in soln.Projects)
			{
				foreach (ProjectItem item in Recurse(project))
				{
					yield return item;
				}
			}
		}

		private static bool LinearMatch(string needle, string haystack)
		{
			int i = 0;
			foreach (char c in haystack)
			{
				if (c == needle[i])
				{
					i++;
				}
				if (i >= needle.Length)
				{
					return true;
				}
			}
			return false;
		}

		private static float RankMatch(string needle, string haystack)
		{
			// Case insensitive search
			needle = needle.ToLower();
			haystack = haystack.ToLower();
			// Quick scan to make sure this is worth ranking
			// Also prevents misordered matches from gaining rank
			if (!LinearMatch(needle, haystack))
			{
				return 0;
			}
			// Can't calculate rank if search pattern is too short
			else if (needle.Length <= 1)
			{
				// We had a match, so we don't want to be filtered out
				return 1;
			}
			float rank = 0;
			// Iterate thru search pattern as [a,b],[b,c] for "abc"
			for (int i = 0; i < needle.Length - 1; i++)
			{
				// Find & store all positions for [a,b] matched in haystack
				List<int> first = new List<int>();
				List<int> second = new List<int>();
				for (int j = 0; j < haystack.Length; j++)
				{
					if(haystack[j] == needle[i])
					{
						first.Add(j);
					}
					if (haystack[j] == needle[i+1])
					{
						second.Add(j);
					}
				}
				// Make sure we've found a & b, and that 'a' comes before 'b'
				if (first.Count > 0 && second.Count > 0 &&
				    first.Min() < second.Max())
				{
					// Find the shortest distance between 'a' & 'b'
					List<int> distances = new List<int>();
					foreach (int dx in first)
					{
						foreach (int dy in second)
						{
							if (dx < dy)
							{
								distances.Add(dy - dx);
							}
						}
					}
					// Rank by shortest distance
					// 5 points for consecutive chars, noticably less as distance increases
					rank += 5.0f / distances.Min();
				}
				else
				{
					return 0;
				}
			}
			return rank;
		}

		public static string GetRelativePath(ProjectItem item)
		{
			string solutionDir = Path.GetDirectoryName(Solution.FullName);
			string filePath = item.Properties.Item("FullPath").Value.ToString();
			var index = filePath.IndexOf(solutionDir);
			return (index < 0) ? filePath : filePath.Remove(index, solutionDir.Length);
		}

		public static List<ProjectItem> RSearch(string word, IEnumerable<ProjectItem> files, double fuzzyness)
		{
			//gets only physical files, not directories.
			List<ProjectItem> foundFiles =
				(
					from s in files
					let rank = (s.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}") ? RankMatch(word, GetRelativePath(s)) : 0
					where rank > 0
					orderby rank descending
					select s
				).Take(7).ToList();

			return foundFiles;
		}
	}
}
