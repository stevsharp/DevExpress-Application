using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CleanS.CleanS
{
    /// <summary>
    /// 
    /// </summary>
    [NonPersistent]
    public abstract class BaseObject : XPLiteObject, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler BeginEditEvent;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSavedEvent;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSavingEvent;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        protected BaseObject(Session session) : base(session){}
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void BeginEdit()
        {
            BeginEditEvent?.Invoke(this, EventArgs.Empty);
            base.BeginEdit();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaved()
        {
            OnSavedEvent?.Invoke(this, EventArgs.Empty);
            base.OnSaved();
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaving()
        {
            OnSavingEvent?.Invoke(this, EventArgs.Empty);
            base.OnSaving();
        }
    }

}
