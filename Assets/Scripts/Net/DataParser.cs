using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

public class DataParser {

	
    public static T ParseData<T>(byte [] data) where T : IMessage, new() {
        IMessage cb = new T();
        T p1;
        p1 = (T)cb.Descriptor.Parser.ParseFrom(data);
        return p1;
    }

    public static byte[] ParseBytes(IMessage message) {
        if(message != null) {
            return message.ToByteArray();
        }
        return null;
    }
}
