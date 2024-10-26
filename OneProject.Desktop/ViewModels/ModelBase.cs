namespace OneProject.Desktop.ViewModels;

using System;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public partial class ModelBase<TModel> : ObservableValidator
    where TModel : class
{
    protected Dispatcher Dispatcher;
    protected IServiceProvider Services;

    protected ModelBase()
    {
        Services = App.Current.Services;
        Dispatcher = Application.Current.Dispatcher;

        Logger = Services.GetRequiredService<ILogger<TModel>>();
    }

    protected ILogger Logger { get; }

    protected virtual T GetService<T>() where T : notnull => Services.GetRequiredService<T>();
}
