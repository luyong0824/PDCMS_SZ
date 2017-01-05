using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Event
{
    public sealed class ActionDelegatedEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        private readonly Action<TEvent> eventHandlerDelegate;

        public ActionDelegatedEventHandler(Action<TEvent> eventHandlerDelegate)
        {
            if (eventHandlerDelegate == null)
            {
                throw new ArgumentNullException("eventHandlerDelegate");
            }

            this.eventHandlerDelegate = eventHandlerDelegate;
        }

        public void Handle(TEvent evt)
        {
            if (evt == null)
            {
                throw new ArgumentNullException("evt");
            }

            eventHandlerDelegate(evt);
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            ActionDelegatedEventHandler<TEvent> otherDelegate = other as ActionDelegatedEventHandler<TEvent>;
            if (otherDelegate == null)
            {
                return false;
            }
            return Delegate.Equals(eventHandlerDelegate, otherDelegate.eventHandlerDelegate);
        }

        public override int GetHashCode()
        {
            return eventHandlerDelegate.GetHashCode();
        }
    }
}
