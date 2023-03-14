using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace CensusRx.WPF.Behaviors;

public class ReadOnlyService : DependencyObject
{
	/// <summary>
	/// IsReadOnly Attached Dependency Property
	/// </summary>
	public static readonly DependencyProperty IsReadOnlyProperty =
		DependencyProperty.RegisterAttached(
			"IsReadOnly",
			typeof(bool),
			typeof(ReadOnlyService),
			new FrameworkPropertyMetadata(false));

	public static bool GetIsReadOnly(DependencyObject d) => (bool)d.GetValue(IsReadOnlyProperty);
	public static void SetIsReadOnly(DependencyObject d, bool value) => d.SetValue(IsReadOnlyProperty, value);
}

public class ReadOnlyRowBehavior : Behavior<DataGrid>
{
	protected override void OnAttached()
	{
		base.OnAttached();

		if (this.AssociatedObject == null)
			throw new InvalidOperationException("AssociatedObject is null");

		AssociatedObject.BeginningEdit += BeginningEdit;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();

		AssociatedObject.BeginningEdit -= BeginningEdit;
	}

	private void BeginningEdit(object? sender, DataGridBeginningEditEventArgs e)
	{
		var isReadOnly = ReadOnlyService.GetIsReadOnly(e.Row);
		if (isReadOnly)
		{
			e.Cancel = isReadOnly;
		}
	}
}