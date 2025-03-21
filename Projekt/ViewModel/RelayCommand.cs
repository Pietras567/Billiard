﻿using System;
using System.Windows.Input;


namespace ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}