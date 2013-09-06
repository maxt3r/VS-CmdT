using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EnvDTE;

namespace Jitbit.CmdT
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MyControl : UserControl
    {
	    public List<ProjectItem> currentFiles { get; set; }
	    public IEnumerable<ProjectItem> ProjectItems { get; set; }
		public event EventHandler ReadyToBeClosed;
        
		public MyControl()
        {
            InitializeComponent();
	        tbSearch.KeyDown += SerchEnterPressed;
			tbSearch.PreviewKeyUp += ArrowKeysPressed;
			Utils u = new Utils();
	        ProjectItems = u.SolutionFiles();

        }

		public void TbSearch_OnLostFocus(object sender, RoutedEventArgs e)
		{
			Close();
		}

	    private void ArrowKeysPressed(object sender, KeyEventArgs e)
	    {
			if (e.Key == Key.Down)
			{
				e.Handled = true;
				if (filesList.SelectedIndex < filesList.Items.Count)
					filesList.SelectedIndex++;
			}

		    if (e.Key == Key.Up)
		    {
				e.Handled = true;
			    if (filesList.SelectedIndex > 0)
				    filesList.SelectedIndex--;
		    }
	    }


	    private void SerchEnterPressed(object sender, KeyEventArgs e)
	    {
			if(e.Key == Key.Enter || e.Key == Key.Return)
				OpenSelectedFile();
	    }

	    private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (String.IsNullOrEmpty(tbSearch.Text))
			{
				filesList.Items.Clear();
				currentFiles.Clear();
				return;
			}

			var list = Utils.RSearch(tbSearch.Text, ProjectItems, 1);
			currentFiles = list;
			filesList.Items.Clear();
			foreach (var l in list)
			{
				var item = new ListBoxItem();
				item.Content = Utils.GetRelativePath(l);
				item.MouseDoubleClick += ListItemClick;
				filesList.Items.Add(item);
			}

			filesList.SelectedIndex = 0;
		}

		void OpenSelectedFile()
		{
			//open the file
			currentFiles[filesList.SelectedIndex].Open().Activate();

			Close();
		}

	    private void Close()
	    {
			//cleaning up
			tbSearch.Text = String.Empty;
			filesList.Items.Clear();
			currentFiles.Clear();

			//hide the window
			if (ReadyToBeClosed != null)
				ReadyToBeClosed(this, null);
	    }

	    void ListItemClick(object sender, MouseButtonEventArgs e)
		{
			OpenSelectedFile();
		}
    }
}