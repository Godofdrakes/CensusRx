using System;
using System.Reactive.Disposables;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using CensusRx.Services;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace CensusRx.WPF.Services;

[ServiceLifetime(ServiceLifetime.Transient)]
public class CreatesCommandBindingViaTextBoxHelper : ICreatesCommandBinding
{
	public int GetAffinityForObject(Type type, bool hasEventTarget)
	{
		return type.IsAssignableTo(typeof(TextBoxBase)) ? 2 : 0;
	}

	public IDisposable? BindCommandToObject(ICommand? command, object? target, IObservable<object?> commandParameter)
	{
		if (target is null)
		{
			throw new ArgumentNullException(nameof(target));
		}

		if (target is not TextBoxBase textBox)
		{
			throw new ArgumentException("target is not a TextBoxBase", nameof(target));
		}

		var oldValue = TextBoxHelper.GetButtonCommand(textBox);

		TextBoxHelper.SetButtonCommand(textBox, command);

		return Disposable.Create(() => TextBoxHelper.SetButtonCommand(textBox, oldValue));
	}

	public IDisposable? BindCommandToObject<TEventArgs>(ICommand? command, object? target, IObservable<object?> commandParameter, string eventName)
	{
		return null; // Not supported
	}
}