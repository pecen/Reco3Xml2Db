using System;
using System.Collections.Generic;
using Unity;
using Unity.Resolution;

namespace Reco3Xml2Db.UI.Module.Services {
  public interface IUnityService {
    /// <summary>
    /// Gets the unitycontainer.
    /// </summary>
    /// <returns>A IUnityContainer instance.</returns>
    IUnityContainer Container { get; }

    /// <summary>
    /// Sets the container.
    /// </summary>
    /// <param name="container">The container.</param>
    /// <returns>MySelf.</returns>
    IUnityService SetContainer(IUnityContainer container);

    /// <summary>
    /// RegisterType an instance with the container.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <param name="instance">The instance to register.</param>
    /// <returns>MySelf.</returns>
    IUnityService RegisterInstance<T>(T instance);

    /// <summary>
    /// RegisterType a type mapping with the container.
    /// </summary>
    /// <param name="from">Type of Interface.</param>
    /// <param name="to">Type of instance.</param>
    /// <param name="name">Name of instance.</param>
    /// <returns>MySelf.</returns>
    IUnityService RegisterType(Type from, Type to, string name);

    /// <summary>
    /// RegisterType a type mapping with given interface to the container.
    /// </summary>
    /// <param name="type">Type of object.</param>
    /// <param name="typeOfInterface">Type of Interface the object are inplemented.</param>
    /// <returns>MySelf.</returns>
    IUnityService RegisterType(Type type, Type typeOfInterface);

    /// <summary>
    /// RegisterType a type mapping with the container.
    /// </summary>
    /// <typeparam name="TFrom">The Interface for the type.</typeparam>
    /// <typeparam name="TTo">Type of instance.</typeparam>
    /// <returns>MySelf.</returns>
    IUnityService RegisterType<TFrom, TTo>() where TTo : TFrom;

    /// <summary>
    /// Get an instance of the requested type from the container.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <param name="overrides">An array with instances that lets you override a named parameter passed to a constructor.</param>
    /// <returns>A instance of type T.</returns>
    T Resolve<T>(params ResolverOverride[] overrides);

    /// <summary>
    /// Get an instance of the requested type from the container.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <param name="name">Name of instance.</param>
    /// <param name="overrides">An array with instances that lets you override a named parameter passed to a constructor.</param>
    /// <returns>A instance of type T.</returns>
    T Resolve<T>(string name, params ResolverOverride[] overrides);

    /// <summary>
    /// Gets all instances of the requested type from the container.
    /// </summary>
    /// <typeparam name="T">Type of instance.</typeparam>
    /// <param name="overrides">An array with instances that lets you override a named parameter passed to a constructor.</param>
    /// <returns>A list of T.</returns>
    IEnumerable<T> ResolveAll<T>(params ResolverOverride[] overrides);
  }
}
