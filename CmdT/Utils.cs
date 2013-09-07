using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
		}

		public IEnumerable<ProjectItem> SolutionFiles()
		{
			DTE2 dte2 = Package.GetGlobalService(typeof(DTE)) as DTE2;
			Solution2 soln = (Solution2)dte2.Solution;
			foreach (Project project in soln.Projects)
			{
				foreach (ProjectItem item in Recurse(project.ProjectItems))
				{
					yield return item;
				}
			}
		}

		private static string Pattern(string src)
		{
			return ".*" + String.Join(".*", src.ToCharArray());
		}

		private static bool RMatch(string src, ProjectItem dest)
		{
			try
			{
				return Regex.Match(GetRelativePath(dest), Pattern(src), RegexOptions.IgnoreCase | RegexOptions.Compiled).Success;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public static string GetRelativePath(ProjectItem item)
		{
			string solutionDir = Path.GetDirectoryName(Solution.FullName);
			string filePath = item.Properties.Item("FullPath").Value.ToString();
			var index = filePath.IndexOf(solutionDir);
			return (index < 0) ? filePath : filePath.Remove(index, solutionDir.Length);
		}

		public static List<ProjectItem> RSearch(string word,IEnumerable<ProjectItem> files, double fuzzyness)
		{
			//gets only physical files, not directories.
			List<ProjectItem> foundFiles =
				(
					from s in files
					where s.Kind=="{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}" && RMatch(word, s) == true 
					orderby s.Name descending 
					select s
				).Take(7).ToList();

			return foundFiles;
		}
	}
}
