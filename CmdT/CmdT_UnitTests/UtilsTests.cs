using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VsSDK.UnitTestLibrary;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jitbit.CmdT;

namespace CmdT_UnitTests
{
	[TestClass()]
	public class UtilsTest
	{
		[TestMethod]
		public void GetsProjectFiles()
		{
			Utils u = new Utils();
			Assert.IsTrue(u.SolutionFiles().Any());
		}
	}
}
