﻿using GoogleMapsComponents.Maps;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    public class ObjectManager
    {
        private readonly IJSRuntime jsRuntime;

        public ObjectManager(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public ValueTask<IJSObjectReference> CreateAsync(
            string constructorFunctionName,
            params object?[] args)
        {
            return jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createObject",
                constructorFunctionName,
                args);
        }

        //public async ValueTask<Dictionary<string, JsObjectRef>> CreateMultipleAsync(
        //    string constructorFunctionName,
        //    IReadOnlyDictionary<string, object[]> pairs)
        //{
        //    //Dictionary<string, Guid> internalMapping = args.ToDictionary(e => e.Key, e => Guid.NewGuid());
        //    //Dictionary<Guid, object> dictArgs = internalMapping.ToDictionary(e => e.Value, e => args[e.Key]);
        //    //Dictionary<Guid, JsObjectRef> result = await CreateMultipleAsync(
        //    //    jsRuntime,
        //    //    constructorFunctionName,
        //    //    dictArgs);

        //    //return internalMapping.ToDictionary(e => e.Key, e => result[e.Value]);

        //    throw new NotImplementedException();
        //}

        //public async static ValueTask<Dictionary<Guid, JsObjectRef>> CreateMultipleAsync(
        //    string functionName,
        //    Dictionary<Guid, object> dictArgs)
        //{
        //    //Dictionary<Guid, JsObjectRef> jsObjectRefs = dictArgs.ToDictionary(e => e.Key, e => new JsObjectRef(jsRuntime, e.Key));

        //    //await jsRuntime.MyInvokeAsync<object>(
        //    //    "googleMapsObjectManager.createMultipleObject",
        //    //    new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
        //    //        .Concat(dictArgs.Values).ToArray()
        //    //);

        //    //return jsObjectRefs;

        //    throw new NotImplementedException();
        //}

        //public ValueTask InvokeMultipleAsync(
        //    string functionName,
        //    Dictionary<IJSObjectReference, object[]> dictArgs)
        //{
        //    return jsRuntime.InvokeVoidAsync(
        //        "googleMapsObjectManager.invokeMultiple",
        //        new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
        //            .Concat(dictArgs.Values).ToArray()
        //    );
        //}

        //public async ValueTask<Dictionary<string, JsObjectRef>> AddMultipleAsync(
        //    string constructorFunctionName,
        //    Dictionary<string, object> args)
        //{
        //    //Dictionary<string, Guid> internalMapping = args.ToDictionary(e => e.Key, e => Guid.NewGuid());
        //    //Dictionary<Guid, object> dictArgs = internalMapping.ToDictionary(e => e.Value, e => args[e.Key]);
        //    //Dictionary<Guid, JsObjectRef> result = await CreateMultipleAsync(
        //    //     _jsRuntime,
        //    //     constructorFunctionName,
        //    //     dictArgs);

        //    //return internalMapping.ToDictionary(e => e.Key, e => result[e.Value]);

        //    throw new NotImplementedException();
        //}

        public ValueTask<IJSObjectReference> AddListenerAsync(
            IJSObjectReference objRef,
            string eventName,
            Action handler)
        {
            return jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.addListenerNoArgument",
                objRef,
                eventName,
                Helper.MakeArgJsFriendly(handler));
        }

        public async ValueTask<IJSObjectReference> AddListenerAsync<T>(
            IJSObjectReference objRef,
            string eventName,
            Action<T> handler)
        {
            var dotNetObjRef = DotNetObjectReference.Create(
                new JsInvokableFunc<T, bool?>(wrapHandler));

            return await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.addListenerWithArgument",
                objRef,
                eventName,
                dotNetObjRef);

            bool? wrapHandler(T obj)
            {
                handler(obj);

                if(obj is MouseEvent mouseEvent)
                {
                    return mouseEvent.StopStatus;
                }

                return null;
            }
        }

        //public async ValueTask AddMultipleListenersAsync(
        //    string eventName,
        //    Dictionary<IJSObjectReference, object> dictArgs)
        //{
        //    var _ = await jsRuntime.MyAddListenerAsync(
        //        "googleMapsObjectManager.addMultipleListeners",
        //        new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), eventName }
        //            .Concat(dictArgs.Values).ToArray()
        //    );

        //    return;
        //}

        //public ValueTask<Dictionary<string, T>> InvokeMultipleAsync<T>(
        //    string functionName,
        //    Dictionary<IJSObjectReference, object> dictArgs)
        //{
        //    return jsRuntime.InvokeAsync<Dictionary<string, T>>(
        //        "googleMapsObjectManager.invokeMultiple",
        //        new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
        //            .Concat(dictArgs.Values).ToArray()
        //    );
        //}

        //public ValueTask<T> ReadObjectPropertyValue<T>(
        //    string propertyName, IJSObjectReference objRef)
        //{
        //    return jsRuntime.InvokeAsync<T>(
        //        "googleMapsObjectManager.readObjectPropertyValue",
        //        objRef,
        //        propertyName);
        //}
    }
}