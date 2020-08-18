//=======================================================
///	Copyright (c) 2018 Launch Design. All Rights Reserved
/// Author:			Happy
/// Time:			2018\6\26 星期二 9:07:27
/// Version:		V0.00.001  
/// Description:	***			
//=======================================================

using System;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Predicate<object> _canExecute;
    private readonly Action<object> _execute;
    readonly bool _checkAuthorization = true;

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<object> execute,
                        Predicate<object> canExecute = null,
                        bool checkAuthorization = true)
    {
        _execute = execute;
        _canExecute = canExecute ?? new Predicate<object>(item => true);
        _checkAuthorization = checkAuthorization;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute(parameter);
    }

    public void Execute(object parameter)
    {
#if !DEBUG
            if (_checkAuthorization)
            {
                if (AuthorizedStatus.Activated != HelpVM.Status)
                {
                    ProductAuthorizationUtility.DetectAppHasBeenAuthorized();
                    return;
                }
            }
#endif
        _execute(parameter);
    }
}

public class RelayCommand<T> : ICommand
{
    private readonly Predicate<T> _canExecute;
    private readonly Action<T> _execute;

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute ?? new Predicate<T>(item => true);
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute((T)parameter);
    }

    public void Execute(object parameter)
    {
        _execute((T)parameter);
    }
}
