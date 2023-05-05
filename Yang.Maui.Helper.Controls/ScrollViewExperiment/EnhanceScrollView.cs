namespace Yang.Maui.Helper.Controls.ScrollViewExperiment;

public class EnhanceScrollView : ScrollView
{
	public EnhanceScrollView()
	{
		Content = new StackLayout
		{
			Children = {
				new Label { Text = "Welcome to .NET MAUI!" }
			}
		};

		this.Scrolled += (sender, e) =>
		{
			var x= e.ScrollX;
			var y= e.ScrollY;
			
		};
	}
}