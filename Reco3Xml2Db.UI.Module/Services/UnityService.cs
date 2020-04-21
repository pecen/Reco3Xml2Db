using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Registration;
using Unity.Resolution;

namespace Reco3Xml2Db.UI.Module.Services {
  /// <summary>
  /// This class handles the configuration of the Dependency injection functionality using Unity application block from EntLib.
  /// </summary>
  public sealed class UnityService : IUnityService {
    /// <summary>
    /// The instance.
    /// Static members are 'eagerly initialized', that is,
    /// immediately when class is loaded for the first time.
    /// .NET guarantees thread safety for static initialization.
    /// </summary>
    private static readonly UnityService _instance = new UnityService();

    /// <summary>
    /// The container
    /// </summary>
    private IUnityContainer _container = new UnityContainer();

    /// <summary>
    /// Name of the section in the config file.
    /// </summary>
    public static readonly string ConfigurationName = "unity";

    /// <summary>
    /// Gets the registrations.
    /// </summary>
    /// <value>
    /// The registrations.
    /// </value>
    public List<IContainerRegistration> Registrations => _container.Registrations.ToList();

    /// <summary>
    /// To make it thread-safe without using locks.
    /// </summary>
    public static UnityService Get() {
      return _instance;
    }

    /// <summary>
    /// Gets the unitycontainer.
    /// </summary>
    /// <returns>A IUnityContainer instance.</returns>
    public IUnityContainer Container {
      get {
        return _container;
      }
    }

    /// <summary>
    /// Get an instance of the requested type from the container.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <param name="overrides">An array with instances that lets you override a named parameter passed to a constructor.</param>
    /// <returns>A instance of type T.</returns>
    public T Resolve<T>(params ResolverOverride[] overrides) {
      return Container.Resolve<T>(overrides);
    }

    /// <summary>
    /// Get an instance of the requested type from the container.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <param name="name">Name of instance.</param>
    /// <param name="overrides">An array with instances that lets you override a named parameter passed to a constructor.</param>
    /// <returns>A instance of type T.</returns>
    public T Resolve<T>(string name, params ResolverOverride[] overrides) {
      return Container.Resolve<T>(name, overrides);
    }

    /// <summary>
    /// Gets all instances of the requested type from the container.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <param name="overrides">An array with instances that lets you override a named parameter passed to a constructor.</param>
    /// <returns>A list of T.</returns>
    public IEnumerable<T> ResolveAll<T>(params ResolverOverride[] overrides) {
      return Container.ResolveAll<T>(overrides);
    }

    /// <summary>
    /// Determines whether this type is registered.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>
    ///   <c>true</c> if this type is registered; otherwise, <c>false</c>.
    /// </returns>
    public bool IsRegistered<T>() {
      lock (Registrations) {
        return Registrations.Find(r => r.RegisteredType == typeof(T) && r.Name == null) != null;
      }
    }

    /// <summary>
    /// Register a type mapping with the container.
    /// </summary>
    /// <typeparam name="TFrom">The Interface for the type.</typeparam>
    /// <typeparam name="TTo">Type of instance.</typeparam>
    /// <returns>Myself.</returns>
    public IUnityService RegisterType<TFrom, TTo>() where TTo : TFrom {
      lock (Registrations) {
        Container.RegisterType<TFrom, TTo>();
      }
      return this;
    }

    /// <summary>
    /// Register a type mapping with the container.
    /// </summary>
    /// <param name="from">Type of Interface.</param>
    /// <param name="to">Type of instance.</param>
    /// <param name="name">Name of instance.</param>
    /// <returns>Myself.</returns>
    public IUnityService RegisterType(Type from, Type to, string name) {
      lock (Registrations) {
        Container.RegisterType(from, to, name);
      }
      return this;
    }

    /// <summary>
    /// Register a type mapping with given interface to the container.
    /// </summary>
    /// <param name="type">Type of object.</param>
    /// <param name="typeOfInterface">Type of Interface the object are inplemented.</param>
    /// <returns>Myself.</returns>
    public IUnityService RegisterType(Type type, Type typeOfInterface) {
      // If the type inplements the Ixxxx<T> interface, add it to the IUnityService.
      type.GetInterfaces()
          .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeOfInterface)
          .ToList().ForEach(x => RegisterType(x, type, type.FullName));
      return this;
    }

    /// <summary>
    /// Register a type mapping with the container, where the created instances will use
    /// the given <see cref="T:Microsoft.Practices.Unity.LifetimeManager" />.
    /// </summary>
    /// <typeparam name="TFrom"><see cref="T:System.Type" /> that will be requested.</typeparam>
    /// <typeparam name="TTo"><see cref="T:System.Type" /> that will actually be returned.</typeparam>
    /// <param name="lifetimeManager">The <see cref="T:Microsoft.Practices.Unity.LifetimeManager" /> that controls the lifetime
    /// of the returned instance.</param>
    /// <param name="injectionMembers">Injection configuration objects.</param>
    /// <returns>The <see cref="T:Microsoft.Practices.Unity.UnityContainer" /> object that this method was called on (this in C#, Me in Visual Basic).</returns>
    public IUnityService RegisterType<TFrom, TTo>(ITypeLifetimeManager lifetimeManager, params InjectionMember[] injectionMembers) where TTo : TFrom {
      lock (Registrations) {
        Container.RegisterType(typeof(TFrom), typeof(TTo), null, lifetimeManager, injectionMembers);
      }
      return this;
    }

    /// <summary>
    /// Register an instance with the container.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <param name="instance">The instance to register.</param>
    /// <returns>Myself.</returns>
    public IUnityService RegisterInstance<T>(T instance) {
      lock (Registrations) {
        Container.RegisterInstance(instance);
      }
      return this;
    }

    /// <summary>
    /// Sets the container.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <returns>Myself.</returns>
    public IUnityService SetContainer(IUnityContainer container) {
      lock (Registrations) {
        _container = container;
      }
      return this;
    }

    // Note: constructor is 'private'
    /// <summary>
    /// Prevents a default instance of the <see cref="UnityService"/> class from being created.
    /// </summary>
    private UnityService() {
    }
  }
}