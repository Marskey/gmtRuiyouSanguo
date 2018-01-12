using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ProtoData<T> where T : class, ProtoBuf.IExtensible
{
    T[] m_dataItems;

    public ProtoData(byte[] buf)
    {
        MemoryStream ms = new MemoryStream(buf);
        BinaryReader br = new BinaryReader(ms);
        string typename = br.ReadString() + ",mw-proto-client";
        uint size = br.ReadUInt32();
        m_dataItems = new T[size];

        System.Type type = System.Type.GetType(typename);
        if (type != typeof(T))
        {
            //Debug.LogError("Type does dot matched.");
            return;
        }


        var s = new mw_serializer0();
        for (int i = 0; i < size; i++)
        {
            int len = br.ReadInt32();
            byte[] itemBuf = br.ReadBytes(len);

            //m_dataItems[i] = ProtoBuf.Serializer.NonGeneric.Deserialize(type, new MemoryStream(itemBuf)) as T;
            m_dataItems[i] = s.Deserialize(new MemoryStream(itemBuf), null, type) as T;

        }

    }

    public int Count
    {
        get
        {
            return m_dataItems.Length;
        }
    }

    public T this[int index]
    {
        get
        {
            return m_dataItems[index];
        }
    }
}
