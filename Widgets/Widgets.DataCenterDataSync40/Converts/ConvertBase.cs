using Modules.FastReflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Widgets.DataCenterDataSync40.Converts
{
    public class ConvertBase<TSource, TTarget> where TTarget : class, new()
    {
        IPropertyAccessor[] sourceAccessors;
        IPropertyAccessor[] targetAccessors;

        public ConvertBase(IFastReflectionFactory<PropertyInfo, IPropertyAccessor> factory)
        {
            PropertyInfo[] sourceProperties = typeof(TSource).GetProperties();
            PropertyInfo[] targetProperties = typeof(TTarget).GetProperties();
            sourceAccessors = sourceProperties.Where(o => targetProperties.Any(p => p.Name == o.Name)).Select(o => factory.Get(o)).ToArray();
            targetAccessors = targetProperties.Where(o => sourceProperties.Any(p => p.Name == o.Name)).Select(o => factory.Get(o)).ToArray();
        }

        public virtual TTarget Convert(TSource source)
        {
            TTarget target = new TTarget();
            foreach (IPropertyAccessor targetAccessor in targetAccessors)
            {
                IPropertyAccessor sourceAccessor = sourceAccessors.First(o => o.Property.Name == targetAccessor.Property.Name);
                targetAccessor.SetValue(target, sourceAccessor.GetValue(source));
            }
            return target;
        }
    }
}
