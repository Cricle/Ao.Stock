using System;
using System.Collections.Generic;

namespace Ao.Stock
{
    public delegate T GetHandler<T>(IIntangibleContext? context);

    public delegate void ConfigHandler<T>(ref T input, IIntangibleContext? context);

    public class DelegateStockEnviroment : IStockIntangible
    {
        public DelegateStockEnviroment()
            : this(new Dictionary<Type, Delegate>(), new Dictionary<Type, Delegate>())
        {
        }

        public DelegateStockEnviroment(Dictionary<Type, Delegate> configHandlers, Dictionary<Type, Delegate> getHandlers)
        {
            ConfigHandlers = configHandlers ?? throw new ArgumentNullException(nameof(configHandlers));
            GetHandlers = getHandlers ?? throw new ArgumentNullException(nameof(getHandlers));
        }

        public Dictionary<Type, Delegate> ConfigHandlers { get; }

        public Dictionary<Type, Delegate> GetHandlers { get; }

        public DelegateStockEnviroment AddConfigHandler<T>(ConfigHandler<T> handler)
        {
            ConfigHandlers[typeof(T)] = handler;
            return this;
        }
        public DelegateStockEnviroment AddGetHandler<T>(GetHandler<T> handler)
        {
            GetHandlers[typeof(T)] = handler;
            return this;
        }

        public void Config<T>(ref T input, IIntangibleContext? context)
        {
            if (ConfigHandlers.TryGetValue(typeof(T), out var del) && del is ConfigHandler<T> hander)
            {
                hander(ref input, context);
            }
            else
            {
                throw new NotSupportedException($"Not support handler {typeof(T)}");
            }
        }

        public T Get<T>(IIntangibleContext? context)
        {
            if (GetHandlers.TryGetValue(typeof(T), out var del) && del is GetHandler<T> hander)
            {
                return hander(context);
            }
            throw new NotSupportedException($"Not support handler {typeof(T)}");
        }
    }
}
