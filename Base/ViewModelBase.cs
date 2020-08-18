//=======================================================
///	Copyright (c) 2018 Launch Design. All Rights Reserved
/// Author:			Happy
/// Time:			2018\6\25 星期一 17:03:59
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Threading;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public Dispatcher WndDispatcher { get; set; }

    public Action CloseAction { get; set; }

    protected RelayCommand _closeCmd = null;

    public virtual RelayCommand CloseWndCmd
    {
        get
        {
            if (null == _closeCmd)
            {
                _closeCmd = new RelayCommand(
                function =>
                {
                    CloseAction?.Invoke();
                });
            }
            return _closeCmd;
        }
    }

    protected void RaiseProperty(string propName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    protected void RaiseProperties(params string[] propNames)
    {
        if (null == propNames || 0 == propNames.Length)
        {
            return;
        }
        foreach (var propName in propNames)
        {
            RaiseProperty(propName);
        }
    }

    protected void SetProperty<T>(ref T field, T value, Expression<Func<T>> expression)
    {
        if (!(expression.Body is MemberExpression body))
        {
            return;
        }

        if (!(body.Member is PropertyInfo propInfo))
        {
            return;
        }

        var propName = propInfo.Name;
        field = value;
        this.RaiseProperty(propName);
    }

    ///// <summary>
    ///// .net framwork 4.7
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="field"></param>
    ///// <param name="value"></param>
    ///// <param name="propName"></param>
    //protected void SetProperty<T>(ref T field, T value, [CallerMemberName]string propName = null)
    //{
    //    if (Equals(field, value))
    //    {
    //        return;
    //    }
    //    field = value;
    //    this.RaiseProperty(propName);
    //}

    protected void SetProperty<T>(ref T field, T value, string propertyName)
    {
        if (Equals(field, value))
        {
            return;
        }
        field = value;
        this.RaiseProperty(propertyName);
    }
}
