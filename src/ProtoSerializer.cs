using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ProtoSerializer
{
    static ProtoSerializer m_serializer = new ProtoSerializer(
        new mw_proto_serializer()
       );
      

    public static ProtoSerializer Instance
    {
        get
        {
            return m_serializer;
        }
    }

    ProtoBuf.Meta.TypeModel[] m_typeModels;

    public ProtoSerializer(params ProtoBuf.Meta.TypeModel[] args)
    {
        m_typeModels = args;
    }

    public object Deserialize(Stream stm, object o, Type t)
    {
        foreach (var s in m_typeModels)
        {
            if (s.IsDefined(t))
                return s.Deserialize(stm, null, t);
        }
        return null;
    }

    public void Serialize(Stream stm, object o)
    {
        foreach (var s in m_typeModels)
        {
            if (s.IsDefined(o.GetType()))
            {
                s.Serialize(stm, o);
                return;
            }
        }
    }
}
