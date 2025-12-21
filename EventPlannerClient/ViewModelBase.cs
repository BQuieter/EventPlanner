using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient
{
    public interface IParameterReceiver
    {
        void ReceiveParameters(object parameters);
        bool RequiresRefresh { get; }
    }

    public abstract class ViewModelBase : IParameterReceiver
    {
        public virtual void ReceiveParameters(object parameters) { }
        public virtual bool RequiresRefresh => false;
    }
}
