#region License statement
// NJamb, a specification and delta-specification DSL
// Copyright (c) 2010-2011, Mark Knell
// Published under the MIT License; all other rights reserved
#endregion

#region using...
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
#endregion

namespace Stile.Testing.SampleTypes
{
    public class NotifyingSample : Sample,
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Frob<TProperty>(Expression<Func<NotifyingSample, TProperty>> expression)
        {
            PropertyChangedEventHandler copy = PropertyChanged;
            if (copy != null)
            {
                var memberExpression = (MemberExpression) expression.Body;
                var propertyInfo = (PropertyInfo) memberExpression.Member;
                propertyInfo.GetValue(this, null); // hack to verify this is a property here
                string propertyName = propertyInfo.Name;
                copy.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return propertyName;
            }
            return null;
        }
    }
}
